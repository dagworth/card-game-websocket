using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class HandCardManager : Node2D {
	public static HandCardManager Instance;

	private Control hand;

	private List<HandCard> hand_cards = [];

	private const int hand_x_offset = 900;
	private const int hand_y_offset = 925;
	private const int hand_spacing = 160;
	private const float hand_angle_increment = 2.5f;
	private const int hand_y_spread_increment = 5;

	public HandCard hover_card;

	public override void _Ready() {
		Instance = this;
		hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
		GameEvents.Instance.HandCardHover += onHoverEnter;
		GameEvents.Instance.HandCardExit += onHoverExit;
	}

	public void addHandCard(CardEntity entity) {
		HandCard card = entity.hand_card;
		card.card_entity = entity;

		hand.AddChild(card);
		card.UpdateStats(entity);
		hand_cards.Add(card);
 
		updateCardPositions();
	}

	public void removeHandCard(int card_id) {
		for (int i = 0; i < hand_cards.Count; i++) {
			if (hand_cards[i].card_entity.id == card_id) {
				hand.RemoveChild(hand_cards[i]);
				hand_cards.RemoveAt(i);
				break;
			}
		}
		updateCardPositions();
	}

	public void onHoverEnter(HandCard card) {
		if (hover_card == null) {
			hover_card = card;
			//updateCardPositions();
		}
	}

	public void onHoverExit(HandCard card) {
		if (hover_card == card) {
			hover_card = null;
			//updateCardPositions();
		}
	}

    private void updateCardPositions() {
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
}
