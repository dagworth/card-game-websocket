struct DelayedEffect(int id, Action effect) {
    public readonly int plr_id = id;
    public readonly Action Effect = effect;
}

public class DelayedEvents(Game game) {
    private readonly Game game = game;

    //if i ever make a delayed effect with more than 5 turns, shoot me
    private readonly List<List<DelayedEffect>> StartTurn = [[],[],[],[],[]];
    private readonly List<List<DelayedEffect>> EndTurn = [[],[],[],[],[]];

    private static void DoList(List<List<DelayedEffect>> list, int plr_id){
        foreach(DelayedEffect d in list[0]){
            if(d.plr_id == plr_id){
                d.Effect.Invoke();
            }
        }
        list.RemoveAt(0);
        list.Add([]);
    }

    public void AddEffect(int plr_id, Action func, Delays type, int cycles){
        switch(type){
            case Delays.EndTurn:
                EndTurn[cycles].Add(new DelayedEffect(
                    plr_id,
                    func
                ));
                break;
            case Delays.EndOppTurn:
                EndTurn[cycles].Add(new DelayedEffect(
                    game.plrs.GetOtherPlayer(plr_id).Id,
                    func
                ));
                break;
            case Delays.StartTurn:
                StartTurn[cycles].Add(new DelayedEffect(
                    plr_id,
                    func
                ));
                break;
            case Delays.StartOppTurn:
                StartTurn[cycles].Add(new DelayedEffect(
                    game.plrs.GetOtherPlayer(plr_id).Id,
                    func
                ));
                break;
        }
    }

    public void DoStartTurnEffects(int plr_id){
        DoList(StartTurn, plr_id);
    }

    public void DoEndTurnEffects(int plr_id){
        DoList(EndTurn, plr_id);
    }
}