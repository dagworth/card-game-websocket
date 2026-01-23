using shared.DTOs;

public interface IHoverable {
    public CardEntity card_entity { get; set; }

    public void UpdateStats(CardEntity card);
}