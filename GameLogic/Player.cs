public class Player(int id) {
    //card_id
    public event Action<int>? OnDraw;

    //card_id
    public event Action<int>? OnPlay;

    private int Id = id;

    public readonly List<CardStatus> Hand = [];
    public readonly List<CardStatus> Void = [];
    public readonly List<CardStatus> Deck = [];

    public readonly List<CardStatus> Board = [];

    private int health = 35;
    private int mana = 0;
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

    public int FindIndex(int id, List<CardStatus> list){
        int i = -1;
        while(++i < list.Count){
            if(list[i].card_id == id){
                return i;
            }
        }
        return i;
    }

    public void PlayCard(int card_id, CardTypes target_type, int target_id){
        int hand_index = FindIndex(card_id, Hand);
        CardStatus card = Hand[hand_index];
        ChangeMana(-card.cost);
        Board.Add(card);
        OnPlay?.Invoke(card.card_id);
        Hand.RemoveAt(hand_index);
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
        return Id;
    }
}