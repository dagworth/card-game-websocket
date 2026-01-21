using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class UIController : Node2D {
	private static Control hand;
    private static Node board;

	private static List<HandCard> hand_cards = [];
	private static List<BoardCard> enemy_board_cards = [];
	private static List<BoardCard> your_board_cards = [];

	private const int hand_x_offset = 900;
	private const int hand_y_offset = 925;
	private const int hand_spacing = 160;
	private const float hand_angle_increment = 2.5f;
	private const int hand_y_spread_increment = 5;

    private const int board_spacing = 160;
	private const int board_x_offset = 400;
	private const int board_y_offset = 500;
	private const int board_y_enemy_offset = 300;

	private const int place_minion_threshold = 700;

	private const string base_hand_card = "res://scenes/hand_card.tscn";
    private const string base_board_card = "res://scenes/board_card.tscn";


	private static bool dragging;
	private static Hoverable hover_card;
	private static HandCard drag_card;
	private static HandCard preview_card;

	public static void addHandCard(CardEntityDTO card) {
		PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_hand_card);
		HandCard clone = loaded_card.Instantiate() as HandCard;
		
        clone.SetUp(card); //set up the card
		hand.AddChild(clone); //put it in the world
		hand_cards.Add(clone); //put it in the list
 
		clone.MouseEntered += () => onHoverEnter(clone);
		clone.MouseExited += () => onHoverExit(clone);
		updateCardPositions();
	}

    public static void addBoardCard(CardEntityDTO card) {
		PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_board_card);
		BoardCard clone = loaded_card.Instantiate() as BoardCard;
		
        clone.SetUp(card); //set up the card
		board.AddChild(clone); //put it in the world
	    your_board_cards.Add(clone); //put it in the list
 
		clone.GetNode<Area2D>("HoverArea").MouseEntered += () => onHoverEnter(clone);
		clone.GetNode<Area2D>("HoverArea").MouseExited += () => onHoverExit(clone);
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

	public static void removeHandCard(int card_id) {
		for (int i = hand_cards.Count - 1; i >= 0; i--) {
			if (hand_cards[i].card_entity.Id == card_id) {
				hand_cards[i].QueueFree();
				hand_cards.RemoveAt(i);
				break;
			}
		}
		updateCardPositions();
	}

	public static void removeBoardCard(int card_id) {
		for (int i = your_board_cards.Count - 1; i >= 0; i--) {
			if (your_board_cards[i].card_entity.Id == card_id) {
				your_board_cards[i].QueueFree();
				your_board_cards.RemoveAt(i);
				break;
			}
		}

		for (int i = enemy_board_cards.Count - 1; i >= 0; i--) {
			if (enemy_board_cards[i].card_entity.Id == card_id) {
				enemy_board_cards[i].QueueFree();
				enemy_board_cards.RemoveAt(i);
				break;
			}
		}
		updateCardPositions();
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
			if (click.IsEcho()) return;

			if (click.IsPressed()) {
				if (hover_card is HandCard) {
					drag_card = (hover_card as HandCard).Duplicate() as HandCard;
					hand.GetParent().AddChild(drag_card);

					dragging = true;
					(hover_card as Control).Visible = false;

					if(preview_card != null) {
						hand.RemoveChild(preview_card);
						preview_card = null;
					}

					Tween tween = CreateTween();
					tween.TweenProperty(drag_card, "rotation_degrees", 0f, 0.2f).SetTrans(Tween.TransitionType.Linear); //change angle to 0
				} else if (hover_card is BoardCard) {
					MessageHandler.ToggleAttack(hover_card.card_entity.Id);
				}
			} else {
				//unclick
				if (dragging) {
					//place the card
					if (click.Position.Y < place_minion_threshold) {
						int card_id = hover_card.card_entity.Id;
						MessageHandler.PlayCard(card_id);
					}
					(hover_card as HandCard).Visible = true;
					dragging = false;
					hover_card = null;
					drag_card.QueueFree();
				}
			}
		}
	}
	
	public override void _Ready() {
		hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
		board = GetTree().Root.GetNode<Node>("Main/Board");
	}

	public override void _Process(double delta) {
		if (dragging) {
			Vector2 pos = GetGlobalMousePosition();
			pos.X -= drag_card.Size.X;
			pos.Y -= drag_card.Size.Y;
			drag_card.Position = pos;
		}
	}

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

        for (int i = 0; i < your_board_cards.Count; i++) {
			BoardCard card = your_board_cards[i];

			float x = board_x_offset + i * board_spacing;
			float y = board_y_offset;

			Vector2 pos = new(x, y);

			Tween tween = card.CreateTween();

			tween.TweenProperty(card, "position", pos, 0.3f).SetTrans(Tween.TransitionType.Quad);
		}

        for (int i = 0; i < enemy_board_cards.Count; i++) {
			BoardCard card = enemy_board_cards[i];

			float x = board_x_offset + i * board_spacing;
			float y = board_y_enemy_offset;

			Vector2 pos = new(x, y);

			Tween tween = card.CreateTween();

			tween.TweenProperty(card, "position", pos, 0.3f).SetTrans(Tween.TransitionType.Quad);
		}
	}
}
