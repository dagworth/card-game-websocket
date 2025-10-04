using System.Buffers;
using Fleck;

public class GameEntity {
    public readonly EventSystem events;
    public readonly CardManager cards;
    public readonly PlayerManager plrs;

    public readonly DelayedEvents delayed;

    public int Id { get; private set; }
    public int Plr_Turn { get; private set; }
    public IGameState Game_State {get; private set; }

    public void ChangeTurn(){
        Plr_Turn = plrs.GetOtherPlayer(Plr_Turn).Id;
    }

    public void SetGameState(IGameState state){
        Game_State = state;
        state.StartState();
    }

    public GameEntity(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        Id = game_id;
        Game_State = new RegularState(this, false);
        Plr_Turn = plr1_id;

        delayed = new DelayedEvents(this);
        events = new EventSystem(this);
        cards = new CardManager(this);
        plrs = new PlayerManager(this, plr1_id, plr2_id);

        plrs.Start(deck1, deck2);
        Console.WriteLine("game made");
    }

    public void MakeCounterableEffect(int plr_id, CardEntity? owner, Action func){
        CardEffect effect = new CardEffect(
            plr_id,
            owner,
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

    public void MakeDelayedEffect(int plr_id, Delays delay_type, Action func, int cycles = 0){
        delayed.AddEffect(plr_id, delay_type, func, cycles);
    }

    public void QueryTargets(int plr_id, Action<List<int>> func, ChooseTargetsParams info){
        //this is here so that the variables in params dont have to be in constructor
        //ease of use for future me
        //and can be changed whenever before filtering
        info.Filter(Id);

        SetGameState(new ChoosingState(
            this,
            plr_id,
            Game_State,
            func,
            info.TargetList
        ));
        MessageHandler.AskForTargets(ServerHandler.GetWSConnection(plr_id), info.TargetList);
    }

    public void PlayerPlayCard(PlayCard data){
        PlayerEntity plr = plrs.GetPlayer(data.PlayerId);
        CardEntity card = cards.GetCard(data.CardId);
        if(Game_State.CanPlayCard(card)){
            plr.PlayCard(data.CardId, data.Targets);
        }
    }

    public void PlayerEndTurn(){
        Game_State.EndTurn();
    }
}