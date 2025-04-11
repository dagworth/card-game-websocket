public class PlayerManager {
    public Game game;
    public Player Plr1 { get; private set; }
    public Player Plr2 { get; private set; }

    public PlayerManager(Game game, int plr1_id, int plr2_id){
        this.game = game;

        Plr1 = new Player(game, plr1_id);
        Plr2 = new Player(game, plr2_id);

        GameManager.AddPlayer(plr1_id, Plr1);
        GameManager.AddPlayer(plr2_id, Plr2);
    }

    public void Start(List<string> deck1, List<string> deck2){
        for(int i = 0; i < deck1.Count; i++){
            Plr1.Deck.Add(game.cards.CreateCard(deck1[i], Plr1.Id));
        }

        for(int i = 0; i < deck2.Count; i++){
            Plr2.Deck.Add(game.cards.CreateCard(deck2[i], Plr2.Id));
        }

        for(int i = 0; i < 5; i++){
            Plr1.DrawCard();
            Plr2.DrawCard();
        }
    }

    public Player GetPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr1;
        return Plr2;
    }

    public Player GetOtherPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr2;
        return Plr1;
    }

    public Player GetOtherPlayer(Player plr){
        if(Plr1 == plr) return Plr2;
        return Plr1;
    }
}