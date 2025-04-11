using System.Buffers;
using Fleck;

public class Game {
    private int card_id_counter = 0;
    public int Id { get; private set; }

    public Player Plr1 { get; private set; }
    public Player Plr2 { get; private set; }

    public readonly Dictionary<int,CardStatus> Cards = [];

    public event Action<int>? OnSacrifice;
    public event Action<int>? OnDeath;
    public event Action<int>? OnAttack;
    public event Action<int>? OnSpawn;

    public int Plr_Turn { get; private set; }
    
    private IGameState Game_State;
    
    public void MakeCounterableEffect(int plr_id, Action func){
        CardEffect effect = new CardEffect(
            plr_id,
            null, //change the ownership of the card effect later, this doesnt matter usually
            func
        );

        if(Game_State is PriorityState same_state){
            same_state.AddEffect(effect, true);
        } else {
            PriorityState new_state = new PriorityState(
                this,
                plr_id,
                Game_State
            );

            new_state.AddEffect(effect, false);
            SetGameState(new_state);
        }
    }

    public void QueryTargets(int plr_id, Action<List<int>> func, ChooseTargetsParams info){
        info.Filter();
        SetGameState(new ChoosingState(
            this,
            plr_id,
            Game_State,
            func,
            info.TargetList
        ));
        MessageHandler.AskForTargets(ServerHandler.GetWSConnection(plr_id), info.TargetList);
    }

    public bool CanPlayCard(int card_id){
        return Game_State.CanPlayCard(GetCard(card_id));
    }

    public void ChangeTurn(){
        Plr_Turn = GetOtherPlayer(Plr_Turn).Id;
    }

    public CardStatus CreateCard(string card_name, int plr_id){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id, this, plr_id, CardStatStorage.GetCardData(card_name));
        Cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int card_id){
        return Cards[card_id];
    }

    public void SetGameState(IGameState state){
        Game_State = state;
        state.StartState();
    }

    public IGameState GetGameState(){
        return Game_State;
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        Id = game_id;
        Game_State = new RegularState(this, false);

        Plr1 = new Player(this, plr1_id);
        Plr2 = new Player(this, plr2_id);

        GameManager.AddPlayer(plr1_id, Plr1);
        GameManager.AddPlayer(plr2_id, Plr2);

        Plr_Turn = plr1_id;

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            Plr1.Deck.Add(CreateCard(deck1[i], plr1_id));
        }

        for(int i = 0; i < deck2.Count; i++){
            Plr2.Deck.Add(CreateCard(deck2[i], plr2_id));
        }

        for(int i = 0; i < 5; i++){
            Plr1.DrawCard();
            Plr2.DrawCard();
        }

        Console.WriteLine("game made");
    }

    public void SpawnCard(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        plr.Board.Add(card);
        card.OnSpawn?.Invoke(this, plr, card); //The spawned card's OnSpawn should happen before other cards
        OnSpawn?.Invoke(card_id);
    }

    public void SacrificeCard(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        int board_id = plr.FindIndex(plr.Board, card_id);
        plr.Board.RemoveAt(board_id);

        plr.Void.Add(card);

        OnSacrifice?.Invoke(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void ReturnFromVoid(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        int void_id = plr.FindIndex(plr.Void, card_id);
        plr.Void.RemoveAt(void_id);

        SpawnCard(card_id);
    }

    public List<CardStatus> GetAllCardsOnBoard(){
        List<CardStatus> card_list = [];
        card_list.AddRange(Plr1.Board);
        card_list.AddRange(Plr2.Board);
        return card_list;
    }

    public Player GetPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr1;
        return Plr2;
    }

    public Player GetOtherPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr2;
        return Plr1;
    }

    public Player GetOtherPlayer(Player plr){
        if(Plr1 == plr) return Plr2;
        return Plr1;
    }

    public void PlayerPlayCard(PlayCard data){
        CardStatus card = GetCard(data.CardId);
        Player plr = GetPlayer(data.PlayerId);
        if(Game_State.CanPlayCard(card)){
            plr.PlayCard(data.CardId);
        }
    }
}