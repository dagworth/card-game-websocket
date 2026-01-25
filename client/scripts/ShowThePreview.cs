using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class ShowThePreview : Node2D {
	private static HandCard preview_card;

	public static void addHandCard(CardEntity entity) {
		HandCard card = entity.hand_card;

		hand.AddChild(card); //put it in the world
		hand_cards.Add(card); //put it in the list
 
		card.MouseEntered += () => onHoverEnter(card);
		card.MouseExited += () => onHoverExit(card);
		updateCardPositions();
	}

	public static void onHoverEnter(IHoverable card) {
		if (hover_card == null && !dragging) {
			hover_card = card;
			if(preview_card == null) {
				preview_card = card.card_entity.preview_card;
				hand.AddChild(preview_card);
			}
			updateCardPositions();
		}
	}

	public static void onHoverExit(IHoverable card) {
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
			if (hand_cards[i].card_entity.id == card_id) {
				hand_cards[i].QueueFree();
				hand_cards.RemoveAt(i);
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
				}
			} else {
				//unclick
				if (dragging) {
					//place the card
					if (click.Position.Y < place_minion_threshold) {
						int card_id = hover_card.card_entity.id;
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
}
