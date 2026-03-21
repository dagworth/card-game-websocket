using Godot;
using shared.DTOs;

public partial class HandCard : Control, IHoverable {
    public CardEntity card_entity { get; set; }
    public HandCard hover_card { get; set; }

    [Export] public RichTextLabel NameLabel;
    [Export] public RichTextLabel DescLabel;
    [Export] public RichTextLabel AttackLabel;
    [Export] public RichTextLabel HealthLabel;
    [Export] public RichTextLabel CostLabel;

    public override void _Ready() {
        MouseEntered += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.HandCardHover, this);
        MouseExited += () => GameEvents.Instance.EmitSignal(GameEvents.SignalName.HandCardExit, this);
    }

    public void UpdateStats(CardEntity card) {
        CardDataDTO data = DataLoader.GetData(card.name);

        NameLabel.Text = card.name;
		DescLabel.Text = data.Description;
		AttackLabel.Text = card.stats.Attack.ToString();
		HealthLabel.Text = card.stats.Health.ToString();
		CostLabel.Text = card.stats.Cost.ToString();
        //GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;
    }
}