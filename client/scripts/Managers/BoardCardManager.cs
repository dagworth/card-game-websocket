using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class BoardCardManager : Node2D {
	public static BoardCardManager Instance;
	
    private Node2D board;

	private List<BoardCard> enemy_board_cards = [];
	private List<BoardCard> your_board_cards = [];

    private const int board_spacing = 160;
	private const int board_x_offset = 400;
	private const int board_y_offset = 500;
	private const int board_y_enemy_offset = 300;

	public BoardCard hover_card;

	public override void _Ready() {
		Instance = this;
		board = GetTree().Root.GetNode<Node2D>("Main/Board");
		GameEvents.Instance.BoardCardHover += onHoverEnter;
		GameEvents.Instance.BoardCardExit += onHoverExit;
	}

    public void addBoardCard(CardEntity entity) {
		BoardCard card = entity.board_card;
		card.card_entity = entity;
		
		card.UpdateStats(entity);
		board.AddChild(card);
		if(entity.plr_id == ClientHandler.plr_id) {
			your_board_cards.Add(card);
		} else {
			enemy_board_cards.Add(card);
		}
 
		updateCardPositions();
	}

	public void removeBoardCard(int card_id) {
		List<BoardCard> search = CardHandler.GetCard(card_id).plr_id == ClientHandler.plr_id ? your_board_cards : enemy_board_cards;

		for (int i = search.Count - 1; i >= 0; i--) {
				if (search[i].card_entity.id == card_id) {
					search[i].QueueFree();
					search.RemoveAt(i);
					break;
				}
			}
		updateCardPositions();
	}

	public void onHoverEnter(BoardCard card) {
		if (hover_card == null) {
			hover_card = card;
			//updateCardPositions();
		}
	}

	public void onHoverExit(BoardCard card) {
		if (hover_card == card) {
			hover_card = null;
			//updateCardPositions();
		}
	}

    private void updateCardPositions() {
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