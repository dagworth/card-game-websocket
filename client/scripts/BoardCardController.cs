using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class BoardCardController : Node2D {
    private static Node board;

	private static List<BoardCard> enemy_board_cards = [];
	private static List<BoardCard> your_board_cards = [];

	private static int units_attacking = 0;

    private const int board_spacing = 160;
	private const int board_x_offset = 400;
	private const int board_y_offset = 500;
	private const int board_y_enemy_offset = 300;

	private static IHoverable hover_card;

    public static void addBoardCard(CardEntity entity) {
		BoardCard card = entity.board_card;

		board.AddChild(card); //put it in the world
		if(entity.plr_id == ClientHandler.plr_id) {
			your_board_cards.Add(card);
		} else {
			enemy_board_cards.Add(card);
		}
 
		card.GetNode<Area2D>("HoverArea").MouseEntered += () => onHoverEnter(card);
		card.GetNode<Area2D>("HoverArea").MouseExited += () => onHoverExit(card);
		updateCardPositions();
	}

	public static void onHoverEnter(IHoverable card) {
		if (hover_card == null) {
			hover_card = card;
			updateCardPositions();
		}
	}

	public static void onHoverExit(IHoverable card) {
		if (hover_card == card) {
			hover_card = null;
			updateCardPositions();
		}
	}

	public static void removeBoardCard(int card_id) {
		for (int i = your_board_cards.Count - 1; i >= 0; i--) {
			if (your_board_cards[i].card_entity.id == card_id) {
				your_board_cards[i].QueueFree();
				your_board_cards.RemoveAt(i);
				break;
			}
		}

		for (int i = enemy_board_cards.Count - 1; i >= 0; i--) {
			if (enemy_board_cards[i].card_entity.id == card_id) {
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
				if((hover_card as BoardCard).card_entity.plr_id != ClientHandler.plr_id) return; //check if ur the right plr

				MessageHandler.ToggleAttack(hover_card.card_entity.id);

				bool attacking = (hover_card as BoardCard).ToggleAttack();
				units_attacking += attacking ? 1 : -1;

				if(units_attacking > 0) {
					GetTree().Root.GetNode<Button>("Main/UI/EndTurn").Text = "Attack";
				} else {
					GetTree().Root.GetNode<Button>("Main/UI/EndTurn").Text = "End Turn";
				}
			}
		}
	}
	
	public override void _Ready() {
		board = GetTree().Root.GetNode<Node>("Main/Board");
	}

    private static void updateCardPositions() {
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