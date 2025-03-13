using Fleck;

public class Game {
    private Player plr1;
    private Player plr2;
    private int game_id;
    private int card_id_counter = 0;
    public readonly Dictionary<int,CardStatus> cards = [];

    //card_id
    public event Action<int>? OnSacrifice;

    //card_id
    public event Action<int>? OnDeath;

    //card_id
    public event Action<int>? OnAttack;

    private int plr_turn;
    private int? plr_action_priority;
    private readonly List<CardStatus> on_hold_cards = [];

    public bool CanPlayCard(int plr_id, int card_id){
        CardStatus card = GetCard(card_id);

        if(plr1.GetMana() < card.cost) return false;

        if(plr_turn != plr_id){
            //if they do not have action priority
            if(plr_action_priority != plr_id){
                return false;
            }
            //will need to check if ambush or fast spell later
        }

        return true;
    }

    public void EndTurn(){
        if(plr1.GetId() == plr_turn){
            plr_turn = plr2.GetId();
        } else {
            plr_turn = plr1.GetId();
        }
    }

    public CardStatus AddCard(string card_name){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id,CardStatStorage.GetCardData(card_name));
        cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int id){
        return cards[id];
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        this.game_id = game_id;

        plr1 = new Player(this, plr1_id);
        plr2 = new Player(this, plr2_id);

        GameManager.AddPlayer(plr1_id, plr1);
        GameManager.AddPlayer(plr2_id, plr2);

        plr_turn = plr1_id;

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            plr1.Deck.Add(AddCard(deck1[i]));
        }
        for(int i = 0; i < deck2.Count; i++){
            plr2.Deck.Add(AddCard(deck2[i]));
        }

        for(int i = 0; i < 3; i++){
            plr1.DrawCard();
            plr2.DrawCard();
        }

        Console.WriteLine($"game was made! {plr1.GetId()} {plr2.GetId()} {plr1.Deck.Count} {plr2.Deck.Count}");
    }

    public bool SacrificeCard(int card_id){
        Player plr = plr1.FindIndex(card_id,plr1.Board) != -1 ? plr1 : plr2;
        int board_id = plr.FindIndex(card_id, plr.Board);

        CardStatus card = plr.Board[board_id];
        plr.Board.RemoveAt(board_id);
        plr.Void.Add(card);

        OnSacrifice?.Invoke(card_id);
        OnDeath?.Invoke(card_id);

        return true;
    }

    public bool ReturnFromVoid(int card_id){
        Player plr = plr1.FindIndex(card_id,plr1.Void) != -1 ? plr1 : plr2;
        int board_id = plr.FindIndex(card_id, plr.Void);

        CardStatus card = plr.Board[board_id];
        plr.Board.RemoveAt(board_id);
        plr.Void.Add(card);

        return true;
    }

    public List<CardStatus> GetAllCardsOnBoard(){
        List<CardStatus> card_list = [];
        card_list.AddRange(plr1.Board);
        card_list.AddRange(plr2.Board);
        return card_list;
    }

    public void PrintState(){
        Console.WriteLine($"\nplr {plr_turn} turn\n");

        Console.WriteLine($"plr {plr1.GetId()} hand");
        foreach(CardStatus card in plr1.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr1.GetId()} board");
        foreach(CardStatus card in plr1.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr2.GetId()} hand");
        foreach(CardStatus card in plr2.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr2.GetId()} board");
        foreach(CardStatus card in plr2.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }
    }
}