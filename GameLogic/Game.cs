using Fleck;

public class Game {
    public readonly ActionExecutor actionExecutor;

    public Player plr1 { get; private set; }
    public Player plr2 { get; private set; }

    private int game_id;
    private int card_id_counter = 0;
    public readonly Dictionary<int,CardStatus> cards = [];

    //card_id
    public event Action<int>? OnSacrifice;

    //card_id
    public event Action<int>? OnDeath;

    //card_id
    public event Action<int>? OnAttack;

    public int plr_turn { get; private set; }
    public GameState game_state { get; set; } = GameState.RegularTurn;

    public int? plr_action_priority { get; private set; } //im going to ignore this as i want to only work on units for now
    private readonly List<CardStatus> on_hold_cards = []; //same with this

    private Action<List<int>>? current_waiting_function; //wait for player to select targets

    public readonly Dictionary<int,List<int>> attacking_units = [];

    public void PlayerChooseTargets(Action<List<int>> func, ChooseTargetsParams info){
        info.Filter();
        current_waiting_function = func;
    }

    public void HandleAttackPhase(){
        foreach (KeyValuePair<int,List<int>> pair in attacking_units){
            GetCard(pair.Key).Attack([..pair.Value.Select(GetCard)]);
        }
    }

    public void EndTurn(){
        if(plr1.id == plr_turn){
            plr_turn = plr2.id;
        } else {
            plr_turn = plr1.id;
        }
    }

    public CardStatus AddCard(string card_name, int plr_id){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id, this, plr_id, CardStatStorage.GetCardData(card_name));
        cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int card_id){
        return cards[card_id];
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        this.game_id = game_id;
        this.actionExecutor = new ActionExecutor(this);

        plr1 = new Player(this, plr1_id);
        plr2 = new Player(this, plr2_id);

        GameManager.AddPlayer(plr1_id, plr1);
        GameManager.AddPlayer(plr2_id, plr2);

        plr_turn = plr1_id;

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            plr1.Deck.Add(AddCard(deck1[i], plr1_id));
        }

        for(int i = 0; i < deck2.Count; i++){
            plr2.Deck.Add(AddCard(deck2[i], plr2_id));
        }

        for(int i = 0; i < 3; i++){
            plr1.DrawCard();
            plr2.DrawCard();
        }

        Console.WriteLine($"game was made! {plr1.id} {plr2.id} {plr1.Deck.Count} {plr2.Deck.Count}");
    }

    public void SacrificeCard(int card_id){
        Player plr = plr1.FindIndex(plr1.Board, card_id) != -1 ? plr1 : plr2;
        int board_id = plr.FindIndex(plr.Board, card_id);

        CardStatus card = plr.Board[board_id];
        plr.Board.RemoveAt(board_id);
        plr.Void.Add(card);

        OnSacrifice?.Invoke(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void ReturnFromVoid(int card_id){
        Player plr = plr1.FindIndex(plr1.Void, card_id) != -1 ? plr1 : plr2;
        int void_id = plr.FindIndex(plr.Void, card_id);
        plr.Void.RemoveAt(void_id);

        plr.SpawnCard(card_id);
    }

    public List<CardStatus> GetAllCardsOnBoard(){
        List<CardStatus> card_list = [];
        card_list.AddRange(plr1.Board);
        card_list.AddRange(plr2.Board);
        return card_list;
    }

    public Player GetPlayer(int plr_id){
        if(plr1.id == plr_id) return plr1;
        return plr2;
    }

    public Player GetOtherPlayer(int plr_id){
        if(plr1.id == plr_id) return plr2;
        return plr1;
    }

    public Player GetOtherPlayer(Player plr){
        if(plr1 == plr) return plr2;
        return plr1;
    }
}