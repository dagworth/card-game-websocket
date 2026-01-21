using Godot;
using shared.DTOs;

public partial class HandCard : Control, Hoverable {
    public CardEntityDTO card_entity { get; set; }
    public HandCard hover_card { get; set; }

    public void SetUp(CardEntityDTO card) {
        card_entity = card;
        CardDataDTO data = DataLoader.GetData(card.Name);

        GetNode<RichTextLabel>("NameLabel").Text = card.Name;
		GetNode<RichTextLabel>("DescLabel").Text = data.Description;
		GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
		GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();
		GetNode<RichTextLabel>("CostLabel").Text = card.Stats.Cost.ToString();
        //GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;

        HandCard preview = Duplicate() as HandCard;
		preview.Position = new Vector2(1000,300);
		preview.Scale = new Vector2(2f,2f);
		hover_card = preview;
    }
}