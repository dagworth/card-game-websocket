public class PlayerManager {
    public GameEntity game;
    public PlayerEntity Plr1 { get; private set; }
    public PlayerEntity Plr2 { get; private set; }

    public PlayerManager(GameEntity game, int plr1_id, int plr2_id){
        this.game = game;

        Plr1 = new PlayerEntity(game, plr1_id);
        Plr2 = new PlayerEntity(game, plr2_id);

        GameManager.AddPlayer(plr1_id, Plr1);
        GameManager.AddPlayer(plr2_id, Plr2);
    }

    public void Start(List<string> deck1, List<string> deck2){
        for(int i = 0; i < deck1.Count; i++){
            CardEntity c = game.cards.CreateCard(deck1[i], Plr1.Id);
            Plr1.Deck.Add(c);
        }

        for(int i = 0; i < deck2.Count; i++){
            CardEntity c = game.cards.CreateCard(deck2[i], Plr2.Id);
            Plr2.Deck.Add(c);
        }

        for(int i = 0; i < 5; i++){
            Plr1.DrawCard();
            Plr2.DrawCard();
        }
    }

    public PlayerEntity GetPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr1;
        return Plr2;
    }

    public PlayerEntity GetOtherPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr2;
        return Plr1;
    }

    public PlayerEntity GetOtherPlayer(PlayerEntity plr){
        if(Plr1 == plr) return Plr2;
        return Plr1;
    }
}