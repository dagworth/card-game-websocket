using Godot;
using shared.DTOs;

public class CardEntity {
    public int id;
    public int plr_id;
    public CardTypes type;
    public string name;
    public CardLocations location;
    public CardStatsDTO stats;

    public HandCard hand_card;
    public BoardCard board_card;

    public HandCard preview_card;

    private const string base_hand_card = "res://scenes/hand_card.tscn";
    private const string base_board_card = "res://scenes/board_card.tscn";

    public CardEntity(CardEntityDTO card) {
        id = card.Id;
        plr_id = card.PlrId;
        type = card.Type;
        name = card.Name;
        location = card.Location;
        stats = card.Stats;

        PackedScene loaded_hand_card = ResourceLoader.Load<PackedScene>(base_hand_card);
		HandCard hand_clone = loaded_hand_card.Instantiate() as HandCard;
        hand_clone.card_entity = this;

        PackedScene loaded_board_card = ResourceLoader.Load<PackedScene>(base_board_card);
		BoardCard board_clone = loaded_board_card.Instantiate() as BoardCard;
        board_clone.card_entity = this;

        HandCard preview = hand_clone.Duplicate() as HandCard;
		preview.Position = new Vector2(1000,300);
		preview.Scale = new Vector2(2f,2f);

        hand_card = hand_clone;
        board_card = board_clone;
		preview_card = preview;

        UpdateCard(card);
    }

    public void UpdateCard(CardEntityDTO card) {
        hand_card.UpdateStats(this);
        board_card.UpdateStats(this);
        preview_card.UpdateStats(this);
    }
}