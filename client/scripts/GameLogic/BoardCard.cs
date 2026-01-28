using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, IHoverable {
	public CardEntity card_entity { get; set; }
	private Sprite2D sprite;

	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite");
		GetNode<Area2D>("HoverArea").MouseEntered += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardHover, this);
        GetNode<Area2D>("HoverArea").MouseExited += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardExit, this);
	}

	public void UpdateStats(CardEntity card) {
		GetNode<RichTextLabel>("NameLabel").Text = card.name;
		GetNode<RichTextLabel>("AttackLabel").Text = card.stats.Attack.ToString();
		GetNode<RichTextLabel>("HealthLabel").Text = card.stats.Health.ToString();
	}

	public void ToggleAttack(bool status) {
		if (status) {
			sprite.Modulate = new Color(0, 1, 1, 1);
		} else {
			sprite.Modulate = new Color(1, 1, 1, 1);
		}
	}
}
