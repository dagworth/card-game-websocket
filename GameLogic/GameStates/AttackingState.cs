public class AttackingState(GameEntity game) : IGameState {
    private readonly GameEntity game = game;
    private readonly int plr_attacking = game.Plr_Turn;
    private readonly Dictionary<int,List<int>> attacking_units = [];

    public void StartState(){}

    public void EndTurn(){
        game.MakeCounterableEffect(
            plr_attacking,
            null,
            () => {
                game.SetGameState(new DefendingState(game, attacking_units));
            }
        );
    }

    public bool CanPlayCard(CardEntity card){ return false; }

    public void ToogleAttack(ToggleAttack data){
        if(plr_attacking != data.PlayerId) return;

        PlayerEntity plr = game.plrs.GetPlayer(plr_attacking);
        if(!plr.Board.Contains(game.cards.GetCard(data.UnitAttacking))) return;

        attacking_units[data.UnitAttacking] = [];
    }

    public void CancelAttack(ToggleAttack data){
        if(plr_attacking != data.PlayerId) return;

        attacking_units.Remove(data.UnitAttacking);

        if(attacking_units.Count == 0){
            game.SetGameState(new RegularState(game, false));
        }
    }
}