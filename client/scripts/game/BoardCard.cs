using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, Hoverable {
    private const string preview_card = "res://scenes/hand_card.tscn";

	public CardEntityDTO card_entity { get; set; }
    public HandCard hover_card { get; set; }

    public void SetUp(CardEntityDTO card) {
        GetNode<RichTextLabel>("NameLabel").Text = card.Name;
		GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
		GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();

        PackedScene loaded_card = ResourceLoader.Load<PackedScene>(preview_card);
		HandCard clone = loaded_card.Instantiate() as HandCard;
        clone.SetUp(card);
		clone.Position = new Vector2(1000,300);
		clone.Scale = new Vector2(2f,2f);
		hover_card = clone;
    }
}
