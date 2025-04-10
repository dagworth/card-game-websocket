public class ChoosingState(Game game, int plr_id, IGameState old_state, Action<List<int>> effect, List<int> choice_pool) : IGameState {
    private readonly Game game = game;
    public int plr_choosing = plr_id;
    private readonly IGameState old_state = old_state;

    private List<int> plr_choice_pool = choice_pool;
    private Action<List<int>> plr_choice_effect = effect;

    public void StateStarted(){}

    public bool CanPlayCard(CardStatus card){
        return false;
    }

    public void EndTurn(){}

    public void GotTargets(TargetsChoice data){
        if(plr_choosing == data.PlayerId){
            bool valid = true;
            foreach (int c in data.Targets){
                if(!plr_choice_pool.Contains(c)){
                    valid = false;
                    break;
                }
            }

            if(!valid) return;

            game.SetGameState(old_state);
            plr_choice_effect.Invoke(data.Targets);
        }
    }
}