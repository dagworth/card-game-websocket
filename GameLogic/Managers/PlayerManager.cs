public class PlayerManager {
    public GameEntity game;
    public PlayerEntity Plr0 { get; private set; }
    public PlayerEntity Plr1 { get; private set; }

    public PlayerManager(GameEntity game, int plr0_id, int plr1_id){
        this.game = game;

        Plr0 = new PlayerEntity(game, plr0_id);
        Plr1 = new PlayerEntity(game, plr1_id);

        GameManager.AddPlayer(plr0_id, Plr0);
        GameManager.AddPlayer(plr1_id, Plr1);
    }

    public void Start(List<string> deck0, List<string> deck1){
        for(int i = 0; i < deck0.Count; i++){
            CardEntity c = game.cards.CreateCard(deck0[i], Plr0.Id);
            Plr0.Deck.Add(c);
        }

        for(int i = 0; i < deck1.Count; i++){
            CardEntity c = game.cards.CreateCard(deck1[i], Plr1.Id);
            Plr1.Deck.Add(c);
        }

        for(int i = 0; i < 5; i++){
            Plr0.DrawCard();
            Plr1.DrawCard();
        }

        game.updater.UpdateClients("game start");
    }

    public PlayerEntity GetPlayer(int plr_id){
        if(Plr0.Id == plr_id) return Plr0;
        return Plr1;
    }

    public PlayerEntity GetOtherPlayer(int plr_id){
        if(Plr0.Id == plr_id) return Plr1;
        return Plr0;
    }

    public PlayerEntity GetOtherPlayer(PlayerEntity plr){
        if(Plr0 == plr) return Plr1;
        return Plr0;
    }
}