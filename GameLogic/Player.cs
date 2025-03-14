public class Player(Game game, int id) {
    //card_id
    public event Action<int>? OnDraw;

    //card_id
    public event Action<int>? OnPlay;

    //card_id
    public event Action<int>? OnSpawn;

    private int id = id;
    private Game game = game;

    public readonly List<CardStatus> Hand = [];
    public readonly List<CardStatus> Void = [];
    public readonly List<CardStatus> Deck = [];

    public readonly List<CardStatus> Board = [];

    private int health = 35;
    private int mana = 100;
    private bool attacked = false;

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

    public void PlayCard(int card_id, List<PlayerTargets> targets){
        int hand_index = FindIndex(Hand, card_id);
        CardStatus card = Hand[hand_index];
        Console.WriteLine($"plr {id} played {card.name}");
        ChangeMana(-card.cost);
        OnPlay?.Invoke(card.card_id);
        Hand.RemoveAt(hand_index);
        SpawnCard(card);
    }

    public void SpawnCard(CardStatus card){
        Board.Add(card);
        OnSpawn?.Invoke(card.card_id);
        card.OnSpawn?.Invoke(game, card);
    }

    public void ChangeHealth(int amount){
        health+=amount;
    }

    public void ChangeMana(int amount){
        mana+=amount;
    }

    public int GetHealth(){
        return health;
    }

    public int GetMana(){
        return mana;
    }

    public int GetId(){
        return id;
    }

    public bool DidAttack(){
        return attacked;
    }

    public void Attacked(){
        attacked = true;
    }

    public Game GetGame(){
        return game;
    }
}