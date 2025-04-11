public class EventSystem(Game game){
    public Game game = game;

    public event Action<int>? OnSacrifice;
    public event Action<int>? OnSpawnFromVoid;
    public event Action<int>? OnIntoVoid;
    public event Action<int>? OnDeath;
    public event Action<int>? OnAttack;
    public event Action<int>? OnSpawn;

    public void SpawnCard(int card_id){
        CardStatus card = game.cards.GetCard(card_id);
        Player plr = game.plrs.GetPlayer(card.plr_id);

        card.location = CardLocations.Board;
        plr.Board.Add(card);
        card.OnSpawn?.Invoke(game, plr, card);
        OnSpawn?.Invoke(card_id);
    }

    public void KillCard(int card_id){
        CardStatus card = game.cards.GetCard(card_id);
        Player plr = game.plrs.GetPlayer(card.plr_id);

        int board_id = plr.FindIndex(plr.Board, card_id);
        plr.Board.RemoveAt(board_id);

        SendToVoid(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void SacrificeCard(int card_id){
        KillCard(card_id);
        OnSacrifice?.Invoke(card_id);
    }

    public void SendToVoid(int card_id){
        CardStatus card = game.cards.GetCard(card_id);
        Player plr = game.plrs.GetPlayer(card.plr_id);
        
        card.location = CardLocations.Void;
        plr.Void.Add(card);

        OnIntoVoid?.Invoke(card_id);
    }

    public void ReturnFromVoid(int card_id){
        CardStatus card = game.cards.GetCard(card_id);
        Player plr = game.plrs.GetPlayer(card.plr_id);

        int void_id = plr.FindIndex(plr.Void, card_id);
        plr.Void.RemoveAt(void_id);

        SpawnCard(card_id);
        OnSpawnFromVoid?.Invoke(card_id);
    }
}