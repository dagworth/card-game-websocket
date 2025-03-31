public class Player(Game game, int id) {
    //card_id
    public event Action<int>? OnDraw;

    //card_id
    public event Action<int>? OnPlay;

    //card_id
    public event Action<int>? OnSpawn;

    public int id { get; private set; } = id;
    public Game game { get; private set; } = game;

    public readonly List<CardStatus> Hand = [];
    public readonly List<CardStatus> Void = [];
    public readonly List<CardStatus> Deck = [];

    public readonly List<CardStatus> Board = [];

    public int health { get; private set; } = 35;
    public int mana { get; private set; } = 100;
    public bool attacked { get; private set; } = false;

    public CardStatus? DrawCard(int index = 0){
        if(Deck.Count == 0 || Deck.Count-1 < index){
            Console.WriteLine("ran out of cards");
            return null;
        }
        CardStatus card = Deck[index];
        Deck.RemoveAt(index);
        Hand.Add(card);
        OnDraw?.Invoke(card.card_id);
        return card;
    }

    public int FindIndex(List<CardStatus> list, int id){
        int i = -1;
        while(++i < list.Count){
            if(list[i].card_id == id){
                return i;
            }
        }
        return i;
    }

    public void PlayCard(int card_id){
        int hand_index = FindIndex(Hand, card_id);
        CardStatus card = Hand[hand_index];
        Console.WriteLine($"plr {id} played {card.name}");
        ChangeMana(-card.cost);
        OnPlay?.Invoke(card.card_id);
        Hand.RemoveAt(hand_index);
        SpawnCard(card_id);
    }

    public void SpawnCard(int card_id){
        CardStatus card = game.GetCard(card_id);
        Board.Add(card);
        card.OnSpawn?.Invoke(game, this, card); //cards' OnSpawn should happen before other cards
        OnSpawn?.Invoke(card_id);
    }

    public void ChangeHealth(int amount){
        health+=amount;
    }

    public void ChangeMana(int amount){
        mana+=amount;
    }

    public void Attacked(){
        attacked = true;
    }
}