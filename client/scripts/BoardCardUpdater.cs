using Godot;
using shared.DTOs;
using System.Collections.Generic;

public partial class BoardCardUpdater : Node2D {
	private static Node board;
	private static List<BoardCard> cards = new();
	private static List<BoardCard> enemy_cards = new();

	private const string base_card = "res://scenes/board_card.tscn";
	private const string hover_base_card = "res://scenes/hand_card.tscn";
	private const int spacing = 160;
	private const int x_offset = 400;
	private const int y_offset = 500;


	public static void addCardFromHand(HandCard card) {
		PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_card);
		BoardCard clone = loaded_card.Instantiate() as BoardCard;

		clone.hover_card = card.hover_card;
		clone.data = card.data;

		addCard(clone);
	}

	public static void addCardFromData(CardDataDTO data) {
		PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_card);
		BoardCard clone = loaded_card.Instantiate() as BoardCard;

		PackedScene loaded_hover = ResourceLoader.Load<PackedScene>(hover_base_card);
		Control preview = loaded_hover.Instantiate() as Control;

		preview.Position = new Vector2(1000,300);
		preview.Scale = new Vector2(2f,2f);

		clone.hover_card = preview;

		addCard(clone);

	}

	private static void addCard(BoardCard clone) {
		CardEntityDTO card = CardHandler.GetCard(clone.data.Id);

		clone.GetNode<RichTextLabel>("NameLabel").Text = card.Name;
		clone.GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
		clone.GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();

		cards.Add(clone);
		board.AddChild(clone);

		updateCardPosition();
	}

	private static void updateCardPosition() {
		for (int i = 0; i < cards.Count; i++) {
			BoardCard card = cards[i];

			float x = x_offset + i * spacing;
			float y = y_offset;

			Vector2 pos = new(x, y);

			Tween tween = card.CreateTween();

			tween.TweenProperty(card, "position", pos, 0.3f).SetTrans(Tween.TransitionType.Quad);
		}
	}

	public override void _Ready() {
		board = GetTree().Root.GetNode<Node>("Main/Board");
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}
}
