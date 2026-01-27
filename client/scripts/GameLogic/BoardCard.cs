using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, IHoverable {
	private const string preview_card = "res://scenes/hand_card.tscn";

	public CardEntity card_entity { get; set; }

	private bool attacking = false;
	private Sprite2D sprite;

	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite");
		// GetNode<Area2D>("HoverArea").MouseEntered += () => {GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardHover, this);GD.Print("in aaaa");};
        // GetNode<Area2D>("HoverArea").MouseExited += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardExit, this);
	}

	public void UpdateStats(CardEntity card) {
		GetNode<RichTextLabel>("NameLabel").Text = card.name;
		GetNode<RichTextLabel>("AttackLabel").Text = card.stats.Attack.ToString();
		GetNode<RichTextLabel>("HealthLabel").Text = card.stats.Health.ToString();
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
