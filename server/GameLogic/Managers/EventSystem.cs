namespace server.GameLogic.Managers;

using server.GameLogic.Entities;

public class EventSystem(GameEntity game) {
    public GameEntity game = game;

    public event Action<int>? OnSacrifice;
    public event Action<int>? OnOutVoid;
    public event Action<int>? OnInVoid;
    public event Action<int>? OnDeath;
    public event Action<int>? OnAttack;
    public event Action<int>? OnSpawn;

    public void SpawnCard(int card_id) {
        CardEntity card = game.cards.GetCard(card_id);
        PlayerEntity plr = game.plrs.GetPlayer(card.Plr_Id);

        card.SetLocation(CardLocations.Board);
        plr.Board.Add(card);
        //Console.WriteLine($"does {card.Name}'s spawn card ability with {plr.Id} or {card.Plr_Id}");
        card.OnSpawn?.Invoke(game, plr, card);
        OnSpawn?.Invoke(card_id);
    }

    public void KillCard(int card_id) {
        CardEntity card = game.cards.GetCard(card_id);
        PlayerEntity plr = game.plrs.GetPlayer(card.Plr_Id);

        plr.Board.Remove(card);

        //Console.WriteLine($"kill {card.Name}");

        SendToVoid(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void SacrificeCard(int card_id) {
        CardEntity card = game.cards.GetCard(card_id);
        //Console.WriteLine($"sacrifice {card.Name}");
        KillCard(card_id);
        OnSacrifice?.Invoke(card_id);
    }

    public void SendToVoid(int card_id) {
        CardEntity card = game.cards.GetCard(card_id);
        PlayerEntity plr = game.plrs.GetPlayer(card.Plr_Id);

        game.updater.ChangeCardLocation(CardLocations.Void, card.Location, card_id, 0);

        card.SetLocation(CardLocations.Void);
        plr.Void.Add(card);

        //Console.WriteLine($"send to void {card.Name}");

        OnInVoid?.Invoke(card_id);
    }

    public CardEntity BringOutVoid(int card_id) {
        CardEntity card = game.cards.GetCard(card_id);
        PlayerEntity plr = game.plrs.GetPlayer(card.Plr_Id);

        game.updater.ChangeCardLocation(CardLocations.Board, CardLocations.Void, card_id, 0);

        card.SetLocation(CardLocations.Board);
        plr.Void.Remove(card);
        plr.Board.Add(card);

        //Console.WriteLine($"bring out of void {card.Name}");

        OnOutVoid?.Invoke(card_id);
        return card;
    }

    public void InvokeOnAttack(int card_id) {
        game.events.OnAttack?.Invoke(card_id);
    }
}