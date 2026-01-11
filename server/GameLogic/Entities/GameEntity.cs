using System.Buffers;
using Fleck;

public class GameEntity {
    public readonly EventSystem events;
    public readonly CardManager cards;
    public readonly PlayerManager plrs;

    public readonly DelayedEvents delayed;

    public readonly UpdaterHandler updater;

    public int Id { get; private set; }
    public int Plr_Turn { get; private set; }
    public IGameState Game_State { get; private set; }

    public void ChangeTurn() {
        Console.WriteLine($"{Plr_Turn} turn ends");
        PlayerEntity new_turn = plrs.GetOtherPlayer(Plr_Turn);
        Plr_Turn = new_turn.Id;
        new_turn.DrawCard();
    }

    public void SetGameState(IGameState state) {
        Game_State = state;
        state.StartState();
    }

    public IDamageable GetTarget(int owner_id, int target_id) {
        if (target_id == -1)
            return plrs.GetPlayer(owner_id);
        if (target_id == -2)
            return plrs.GetOtherPlayer(owner_id);

        return cards.GetCard(target_id);
    }

    public GameEntity(int game_id, int plr0_id, int plr1_id, List<string> deck0, List<string> deck1) {
        Id = game_id;
        Game_State = new RegularState(this, false);
        Plr_Turn = plr0_id;

        delayed = new DelayedEvents(this);
        events = new EventSystem(this);
        cards = new CardManager(this);
        plrs = new PlayerManager(this, plr0_id, plr1_id);
        updater = new UpdaterHandler(this);

        plrs.Start(deck0, deck1);
        Console.WriteLine($"game made with {plr0_id} and {plr1_id}");
    }

    public void MakeCounterableEffect(int plr_id, CardEntity? owner, Action func) {
        CardEffect effect = new CardEffect(plr_id, owner, func);

        if (Game_State is PriorityState same_state) {
            same_state.AddEffect(effect, true);
        } else {
            PriorityState new_state = new PriorityState(this, plr_id, Game_State);

            new_state.AddEffect(effect, false);
            SetGameState(new_state);
        }
    }

    public void MakeDelayedEffect(int plr_id, Delays delay_type, Action func, int cycles = 0) {
        delayed.AddEffect(plr_id, delay_type, func, cycles);
    }

    public void QueryTargets(int plr_id, Action<List<int>> func, ChooseTargetsParams info) {
        //this is here so that the variables in params dont have to be in constructor
        //ease of use for future me
        //and can be changed whenever before filtering
        info.Filter(Id);

        SetGameState(new ChoosingState(this, plr_id, Game_State, func, info.TargetList));
        MessageHandler.AskForTargets(ServerHandler.GetWSConnection(plr_id), info.TargetList);
    }

    public void PlayerPlayCard(PlayCard data) {
        PlayerEntity plr = plrs.GetPlayer(data.PlayerId);
        CardEntity card = cards.GetCard(data.CardId);

        if (Game_State.CanPlayCard(card)) {
            updater.ChangeCardLocation(CardLocations.Board, CardLocations.Hand, data.CardId);
            plr.PlayCard(data.CardId, data.Targets);
            updater.UpdateClients("played card");
        }
    }

    public void PlayerEndTurn() {
        Game_State.EndTurn();
    }
}
