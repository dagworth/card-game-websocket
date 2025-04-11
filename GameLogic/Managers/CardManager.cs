public class CardManager(Game game){
    public Game game = game;
    public readonly Dictionary<int,CardStatus> Cards = [];
    private int card_id_counter = 0;

    public CardStatus CreateCard(string card_name, int plr_id){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id, game, plr_id, CardStatStorage.GetCardData(card_name));
        Cards[id] = new_card;
        new_card.custom_effects?.Invoke(game, game.plrs.GetPlayer(plr_id), new_card);
        return new_card;
    }

    public CardStatus GetCard(int card_id){
        return Cards[card_id];
    }
}