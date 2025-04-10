public class AttackingState(Game game) : IGameState {
    private readonly Game game = game;
    private readonly int plr_attacking = game.Plr_Turn;
    private readonly Dictionary<int,List<int>> attacking_units = [];

    public void StateStarted(){}

    public bool CanPlayCard(CardStatus card){
        return false;
    }

    public void EndTurn(){
        game.SetGameState(new DefendingState(game, attacking_units));
    }

    public void ToogleAttack(ToggleAttack data){
        if(plr_attacking != data.PlayerId) return;

        Player plr = game.GetPlayer(plr_attacking);
        if(plr.FindIndex(plr.Board, data.UnitAttacking) == -1) return;

        attacking_units[data.UnitAttacking] = [];
    }

    public void CancelAttack(ToggleAttack data){
        if(plr_attacking != data.PlayerId) return;

        attacking_units.Remove(data.UnitAttacking);

        if(attacking_units.Count == 0){
            game.SetGameState(new RegularState(game));
        }
    }
}