using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class HandCardUpdater : Node2D {
	private static Control hand;
	private static List<HandCard> hand_cards = [];
	private static List<BoardCard> enemy_board = [];
	private static List<BoardCard> your_board = [];

	private const int hand_x_offset = 900;
	private const int hand_y_offset = 925;
	private const int hand_spacing = 160;
	private const float hand_angle_increment = 2.5f;
	private const int hand_y_spread_increment = 5;

	private const int place_minion_threshold = 700;



	private const string base_card = "res://scenes/hand_card.tscn";

	private static bool dragging;
	private static HandCard hover_card;
	private static HandCard drag_card;
	private static HandCard preview_card;

	private static void updateCardPositions() {
		int count = hand_cards.Count;
		int side_index = count / 2;

		for (int i = 0; i < hand_cards.Count; i++) {
			Control card = hand_cards[i];
			card.ZIndex = 0;

			float x = hand_x_offset + (side_index - i) * -hand_spacing;
			float y = hand_y_offset + Math.Abs(side_index - i) * hand_y_spread_increment;
			float angle = (side_index - i) * -hand_angle_increment;

			Vector2 pos = new(x, y);

			Tween tween = card.CreateTween();

			tween.TweenProperty(card, "position", pos, 0.3f).SetTrans(Tween.TransitionType.Quad);
			tween.Parallel().TweenProperty(card, "rotation_degrees", angle, 0.3f).SetTrans(Tween.TransitionType.Quad);
		}
	}

	public static void addHandCard(int card_id) {
		CardEntityDTO card = CardHandler.GetCard(card_id);
		CardDataDTO data = DataLoader.GetData(card.Name);

		PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_card);

		HandCard clone = loaded_card.Instantiate() as HandCard;
		clone.data = card;
		clone.GetNode<RichTextLabel>("NameLabel").Text = card.Name;
		clone.GetNode<RichTextLabel>("DescLabel").Text = data.Description;
		clone.GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
		clone.GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();
		clone.GetNode<RichTextLabel>("CostLabel").Text = card.Stats.Cost.ToString();
		//clone.GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;
		
		hand.AddChild(clone);
		clone.SetUp();
		hand_cards.Add(clone);

		clone.MouseEntered += () => onHoverEnter(clone);
		clone.MouseExited += () => onHoverExit(clone);
		updateCardPositions();
	}

	public static void removeHandCard(int card_id) {
		for (int i = hand_cards.Count - 1; i >= 0; i--) {
			if (hand_cards[i].data.Id == card_id) {
				hand_cards[i].QueueFree();
				hand_cards.RemoveAt(i);
				break;
			}
		}
		updateCardPositions();
	}

	public static void onHoverEnter(Hoverable card) {
		if (hover_card == null && !dragging) {
			hover_card = card;
			if(preview_card == null) {
				preview_card = card.hover_card;
				hand.AddChild(preview_card);
			}
			updateCardPositions();
		}
	}

	public static void onHoverExit(Hoverable card) {
		if (hover_card == card && !dragging) {
			hover_card = null;
			if(preview_card != null) {
				hand.RemoveChild(preview_card);
				preview_card = null;
			}
			updateCardPositions();
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
			if (click.IsEcho()) return;

			if (click.IsPressed()) {
				if (hover_card != null) {
					drag_card = hover_card.Duplicate() as HandCard;
					hand.GetParent().AddChild(drag_card);

					dragging = true;
					hover_card.Visible = false;

					Tween tween = CreateTween();
					tween.TweenProperty(drag_card, "rotation_degrees", 0f, 0.2f).SetTrans(Tween.TransitionType.Linear);
				}
			} else {
				if (dragging) {
					if (click.Position.Y < place_minion_threshold) {
						int card_id = (hover_card as HandCard).data.Id;
						GD.Print($"try to place {card_id}");
						MessageHandler.PlayCard(card_id);
						BoardCardUpdater.addCardFromHand(hover_card as HandCard);
						hand_cards.Remove(hover_card);
						hover_card.QueueFree();
						if(preview_card != null) {
							hand.RemoveChild(preview_card);
							preview_card = null;
						}
					} else {
						hover_card.Visible = true;
					}
					hover_card.Visible = true;
					dragging = false;
					hover_card = null;
					drag_card.QueueFree();
				}
			}
		} else if (@event is InputEventMouseMotion move) {
			if (drag_card != null && !dragging) {
				if (move.Position.Y < place_minion_threshold) {
					drag_card = null;
					updateCardPositions();
				}
			}
		}
	}
	
	public override void _Ready() {
		hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
	}

	public override void _Process(double delta) {
		if (dragging) {
			Vector2 pos = GetGlobalMousePosition();
			pos.X -= drag_card.Size.X;
			pos.Y -= drag_card.Size.Y;
			drag_card.Position = pos;
		}
	}
}
