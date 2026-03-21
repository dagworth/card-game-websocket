using Godot;
using shared.DTOs;
using System.Collections.Generic;

public partial class CardHandler : Node {
	public static Dictionary<int, CardEntity> cards = [];

	public static void AddCard(CardEntityDTO card) {
		CardEntity clone = new(card);
		cards[card.Id] = clone;
		HandCardManager.Instance.addHandCard(clone);
	}

	public static CardEntity GetCard(int id) {
		if (cards.TryGetValue(id, out CardEntity entity)) {
			return entity;
		}
		//GD.Print($"plr {ClientHandler.plr_id} does not know about card {id}");
		return null;
	}
}
