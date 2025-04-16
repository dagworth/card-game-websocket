using Fleck;

public class PlayerEntity(GameEntity game, int id) {
    
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
    public bool Attacked { get; private set; } = false;

    public CardEntity? DrawCard(int index = 0){
        if(Deck.Count == 0 || Deck.Count-1 < index){
            Console.WriteLine("ran out of cards");
            return null;
        }
        CardEntity card = Deck[index];
        Deck.RemoveAt(index);
        card.SetLocation(CardLocations.Hand);
        Hand.Add(card);
        OnDraw?.Invoke(card.Id);
        return card;
    }

    public void PlayCard(int card_id, List<int> targets){
        CardEntity card = Game.cards.GetCard(card_id);
        Console.WriteLine($"plr {Id} played {card.Name}");
        ChangeMana(-card.Stats.Cost);
        Hand.Remove(card);
        if(card.Type == CardTypes.Unit){
            Game.events.SpawnCard(card_id);
        } else {
            card.SetLocation(CardLocations.Void);
            Void.Add(card);
        }
        card.OnPlay?.Invoke(Game,this,card,targets); //plz fix onplay targetting
        OnPlay?.Invoke(card.Id);
    }

    public void ChangeHealth(int amount){
        Health+=amount;
    }

    public void ChangeMana(int amount){
        Mana+=amount;
    }
}