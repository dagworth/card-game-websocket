namespace server.GameLogic.GameStates;

using server.GameLogic.Entities;
using server.GameLogic.Interfaces;
using shared;

public class AttackingState(GameEntity game) : IGameState {
    private readonly GameEntity game = game;
    private readonly int plr_attacking = game.Plr_Turn;
    private readonly Dictionary<int, HashSet<int>> attacking_units = [];

    public void StartState() { }

    public void EndTurn(EndTurnRequest req) {
        if(req.PlayerId != plr_attacking) return;
        game.MakeCounterableEffect(
            plr_attacking,
            null,
            () => {
                game.SetGameState(new DefendingState(game, attacking_units));
            }
        );
    }

    public bool CanPlayCard(CardEntity card) { return false; }

    public void ToogleAttack(ToggleAttackRequest data) {
        if(attacking_units.ContainsKey(data.UnitAttacking)) {
            CancelAttack(data);
            return;
        }
        if (plr_attacking != data.PlayerId) return;

        PlayerEntity plr = game.plrs.GetPlayer(plr_attacking);
        if (!plr.Board.Contains(game.cards.GetCard(data.UnitAttacking))) return;

        game.updater.ToggleAttack(data.UnitAttacking, true);
        game.updater.UpdateClients();
        attacking_units[data.UnitAttacking] = [];
    }

    public void CancelAttack(ToggleAttackRequest data) {
        if (plr_attacking != data.PlayerId) return;

        game.updater.ToggleAttack(data.UnitAttacking, false);
        game.updater.UpdateClients();
        attacking_units.Remove(data.UnitAttacking);

        if (attacking_units.Count == 0) {
            game.SetGameState(new RegularState(game, false));
        }
    }
}