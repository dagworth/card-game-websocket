using Godot;
using shared.DTOs;

public partial class HandCard : Control, Hoverable {
    public CardEntityDTO data;
    public HandCard hover_card;

    public void SetUp() {
        HandCard preview = Duplicate() as HandCard;
		preview.Position = new Vector2(1000,300);
		preview.Scale = new Vector2(2f,2f);
		hover_card = preview;
    }
}