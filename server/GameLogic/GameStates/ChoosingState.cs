namespace server.GameLogic.GameStates;

using server.GameLogic.Entities;
using server.GameLogic.Interfaces;
using shared;

public class ChoosingState(GameEntity game, int plr_id, IGameState old_state, Action<List<int>> effect, List<int> choice_pool) : IGameState {
    private readonly GameEntity game = game;
    public int plr_choosing = plr_id;
    private readonly IGameState old_state = old_state;

    private List<int> plr_choice_pool = choice_pool;
    private Action<List<int>> plr_choice_effect = effect;

    public void StartState() { }
    public void EndTurn() { }

    public bool CanPlayCard(CardEntity card) {
        return false;
    }

    public void GotTargets(TargetsChoice data) {
        if (plr_choosing == data.PlayerId) {
            bool valid = true;
            foreach (int c in data.Targets) {
                if (!plr_choice_pool.Contains(c)) {
                    valid = false;
                    break;
                }
            }

            if (!valid) {
                Console.WriteLine("chosen stuff is invalid.. ignoring effect");
                game.SetGameState(old_state);
                return;
            }

            game.SetGameState(old_state);
            //Console.WriteLine("before targetting effect");
            plr_choice_effect.Invoke(data.Targets);
            //Console.WriteLine("after targetting effect");
            //this needs to be in this order
            //because of the case of needing to choose when in a priority turn
            //which has an effect that adds a counterable effect
            //the priority state has to be in place before that happens
            game.updater.UpdateClients("choose targets");
            //Console.WriteLine("updated clients with targetting effect");
        }
    }
}