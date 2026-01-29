namespace server.GameLogic.Entities;

using server.GameLogic.Interfaces;

public class PlayerEntity(GameEntity game, int id) : IDamageable {

    public event Action<int>? OnDraw;
    public event Action<int>? OnPlay;

    public int Id { get; private set; } = id;
    public GameEntity Game { get; private set; } = game;

    public readonly List<CardEntity> Hand = [];
    public readonly List<CardEntity> Void = [];
    public readonly List<CardEntity> Deck = [];

    public readonly List<CardEntity> Board = [];

    public int Health { get; private set; } = 35;
    public int Mana { get; private set; } = 100;
    public int MaxMana { get; private set; } = 100;
    public bool Attacked { get; private set; } = false;

    public CardEntity? DrawCard(int index = 0) {
        if (Deck.Count == 0 || Deck.Count - 1 < index) {
            Console.WriteLine("ran out of cards");
            return null;
        }
        CardEntity card = Deck[index];
        Deck.RemoveAt(index);
        card.SetLocation(CardLocations.Hand);
        Game.updater.NewCard(card);
        Hand.Add(card);
        OnDraw?.Invoke(card.Id);
        return card;
    }

    public void PlayCard(int card_id) {
        CardEntity card = Game.cards.GetCard(card_id);
        Console.WriteLine($"plr {Id} played {card.Name}");
        ChangeMana(-card.Stats.Cost);
        Hand.Remove(card);
        Game.updater.NewCard(card,true);
        if (card.Type == CardTypes.Unit) {
            Game.updater.ChangeCardLocation(CardLocations.Board, CardLocations.Hand, card_id);
            Game.events.SpawnCard(card_id);
        } else {
            Game.updater.ChangeCardLocation(CardLocations.Void, CardLocations.Hand, card_id);
            card.SetLocation(CardLocations.Void);
            Void.Add(card);
        }
        card.OnPlay?.Invoke(Game, this, card);
        OnPlay?.Invoke(card.Id);
    }

    public void TakeDamage(int amount) {
        Health -= amount;
        Game.updater.PlrTookDamage(Id, amount);
        Game.updater.UpdateClients();
    }

    public void ChangeHealth(int amount) {
        Health += amount;
        Game.updater.PlrTookDamage(Id, -amount);
        Game.updater.UpdateClients();
    }

    public void ChangeMana(int amount) {
        Mana += amount;
        Game.updater.ManaChange(Id, MaxMana, Mana);
        Game.updater.UpdateClients();
    }
}