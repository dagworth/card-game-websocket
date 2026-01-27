using Godot;
using shared.DTOs;
using System.Collections.Generic;

public partial class CardHandler : Node {
	public static Dictionary<int, CardEntity> cards = [];

	public static void AddCard(CardEntityDTO card) {
		GD.Print(ClientHandler.plr_id + " added " + card.Id);
		CardEntity clone = new(card);
		cards[card.Id] = clone;
		HandCardManager.Instance.addHandCard(clone);
	}

	public static CardEntity GetCard(int id) {
		return cards[id];
	}
}
