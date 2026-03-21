using System.ComponentModel;
using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, IHoverable {
	public CardEntity card_entity { get; set; }
	
	[Export] public Sprite2D sprite;
	[Export] public Area2D HoverArea;
	[Export] public RichTextLabel NameLabel;
	[Export] public RichTextLabel AttackLabel;
	[Export] public RichTextLabel HealthLabel;

	public override void _Ready() {
		HoverArea.MouseEntered += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardHover, this);
        HoverArea.MouseExited += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.BoardCardExit, this);
	}

	public void UpdateStats(CardEntity card) {
		NameLabel.Text = card.name;
		AttackLabel.Text = card.stats.Attack.ToString();
		HealthLabel.Text = card.stats.Health.ToString();
	}

	public void ToggleAttack(bool status) {
		if (status) {
			sprite.Modulate = new Color(0, 1, 1, 1);
		} else {
			sprite.Modulate = new Color(1, 1, 1, 1);
		}
	}
}
