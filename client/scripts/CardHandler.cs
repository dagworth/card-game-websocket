using Godot;
using shared.DTOs;
using System.Collections.Generic;

public partial class CardHandler : Node {
    public static Dictionary<int, CardEntityDTO> cards = [];

    public static void AddCard(CardEntityDTO card) {
        GD.Print("added " + card.Id);
        cards[card.Id] = card;
        UIController.addHandCard(card.Id);
    }

    public static CardEntityDTO GetCard(int id) {
        return cards[id];
    }
}