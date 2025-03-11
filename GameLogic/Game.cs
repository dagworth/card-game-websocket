using Fleck;
public class Game {
    private int game_id;

    //card_id
    public event Action<int>? OnSacrifice;

    //card_id
    public event Action<int>? OnDeath;

    //card_id
    public event Action<int>? OnAttack;

    private Player plr1;
    private Player plr2;

    private bool plr1_turn = true;

    private int card_id_counter = 0;
    public readonly Dictionary<int,CardStatus> cards = [];
    private CardStatus? on_hold_card; //so fast spells

    public CardStatus AddCard(string card_name){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id,CardStatStorage.GetCardData(card_name));
        cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int id){
        return cards[id];
    }

    public bool IsPlayerTurn(int plr_id){
        if(plr_id == plr1.GetId()){
            return plr1_turn;
        } else if(plr_id == plr2.GetId()){
            return !plr1_turn;
        } else {
            Console.WriteLine("this player is not in this game");
            return false;
        }
    }

    public Player GetPlayer(int plr_id){
        if(plr1.GetId() == plr_id){
            return plr1;
        } else if(plr2.GetId() == plr_id){
            return plr2;
        } else {
            Console.WriteLine("this player is not in this game");
            return plr1;
        }
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        this.game_id = game_id;

        plr1 = new Player(plr1_id);
        plr2 = new Player(plr2_id);

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            plr1.Deck.Add(AddCard(deck1[i]));
        }
        for(int i = 0; i < deck2.Count; i++){
            plr2.Deck.Add(AddCard(deck2[i]));
        }

        Console.WriteLine($"game was made! {plr1.GetId()} {plr2.GetId()} {plr1.Deck.Count} {plr2.Deck.Count}");
    }

    public void EndTurn(){
        plr1_turn = !plr1_turn;
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
}