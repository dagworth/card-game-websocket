using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, Hoverable {
	private const string preview_card = "res://scenes/hand_card.tscn";

	public CardEntityDTO card_entity { get; set; }
	public HandCard hover_card { get; set; }

	private bool attacking = false;
	private Sprite2D sprite;

	public void SetUp(CardEntityDTO card) {
		sprite = GetNode<Sprite2D>("Sprite");
		card_entity = card;
		
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

	public bool ToggleAttack() {
		attacking = !attacking;
		if (attacking) {
			sprite.Modulate = new Color(0, 1, 1, 1);
		} else {
			sprite.Modulate = new Color(1, 1, 1, 1);
		}
		return attacking;
	}
}
