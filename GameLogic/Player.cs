using Fleck;

public class Player(Game game, int id) {
    
    public event Action<int>? OnDraw;
    public event Action<int>? OnPlay;

    public int Id { get; private set; } = id;
    public Game Game { get; private set; } = game;

    public readonly List<CardStatus> Hand = [];
    public readonly List<CardStatus> Void = [];
    public readonly List<CardStatus> Deck = [];

    public readonly List<CardStatus> Board = [];

    public int Health { get; private set; } = 35;
    public int Mana { get; private set; } = 100;
    public bool Attacked { get; private set; } = false;

    public CardStatus? DrawCard(int index = 0){
        if(Deck.Count == 0 || Deck.Count-1 < index){
            Console.WriteLine("ran out of cards");
            return null;
        }
        CardStatus card = Deck[index];
        Deck.RemoveAt(index);
        card.SetLocation(CardLocations.Hand);
        Hand.Add(card);
        OnDraw?.Invoke(card.Id);
        return card;
    }

    public int FindIndex(List<CardStatus> list, int id){
        int i = -1;
        while(++i < list.Count){
            if(list[i].Id == id){
                return i;
            }
        }
        return i;
    }

    public void PlayCard(int card_id){
        int hand_index = FindIndex(Hand, card_id);
        CardStatus card = Hand[hand_index];
        Console.WriteLine($"plr {Id} played {card.Name}");
        ChangeMana(-card.Cost);
        Hand.RemoveAt(hand_index);
        if(card.Type == CardTypes.Unit){
            Game.events.SpawnCard(card_id);
        } else {
            card.SetLocation(CardLocations.Void);
            Void.Add(card);
        }
        card.OnPlay?.Invoke(Game,this,card,[]); //plz fix onplay targetting
        OnPlay?.Invoke(card.Id);
    }

    public void ChangeHealth(int amount){
        Health+=amount;
    }

    public void ChangeMana(int amount){
        Mana+=amount;
    }
}