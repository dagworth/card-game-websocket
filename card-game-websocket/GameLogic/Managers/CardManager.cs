public class CardManager(GameEntity game) {
    public GameEntity game = game;
    public readonly Dictionary<int, CardEntity> Cards = [];
    private int card_id_counter = 0;

    public CardEntity CreateCard(string card_name, int plr_id) {
        int id = card_id_counter++;
        CardEntity new_card = new CardEntity(game, plr_id, id, card_name, CardStatStorage.GetCardData(card_name));
        new_card.UpdateStats();
        Cards[id] = new_card;
        new_card.custom_effects?.Invoke(game, game.plrs.GetPlayer(plr_id), new_card);
        return new_card;
    }

    public CardEntity GetCard(int card_id) {
        return Cards[card_id];
    }

    public void DoToAllCards(Action<CardEntity> func) {
        int count = 0;
        foreach (KeyValuePair<int, CardEntity> a in Cards) {
            count++;
            func.Invoke(a.Value);
        }
        Console.WriteLine("count " + count);
    }
}