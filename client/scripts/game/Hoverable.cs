using shared.DTOs;

public interface Hoverable {
    public CardEntityDTO card_entity { get; set; }
    public HandCard hover_card { get; set; }

    public void SetUp(CardEntityDTO card);
}