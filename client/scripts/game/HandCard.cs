using Godot;
using shared.DTOs;

public partial class HandCard : Control, IHoverable {
    public CardEntity card_entity { get; set; }
    public HandCard hover_card { get; set; }

    public void UpdateStats(CardEntity card) {
        CardDataDTO data = DataLoader.GetData(card.name);

        GetNode<RichTextLabel>("NameLabel").Text = card.name;
		GetNode<RichTextLabel>("DescLabel").Text = data.Description;
		GetNode<RichTextLabel>("AttackLabel").Text = card.stats.Attack.ToString();
		GetNode<RichTextLabel>("HealthLabel").Text = card.stats.Health.ToString();
		GetNode<RichTextLabel>("CostLabel").Text = card.stats.Cost.ToString();
        //GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;
    }
}