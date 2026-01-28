namespace server.GameLogic.GameStates;

using server.GameLogic.Entities;
using server.GameLogic.Interfaces;
using shared;

public class DefendingState : IGameState {
    private readonly GameEntity game;
    private readonly int plr_defending;
    private readonly Dictionary<int, HashSet<int>> attacking_units;
    private int defending_units = 0;

    public DefendingState(GameEntity game, Dictionary<int, HashSet<int>> attacking_units) {
        this.game = game;
        plr_defending = game.plrs.GetOtherPlayer(game.Plr_Turn).Id;
        this.attacking_units = attacking_units;

        PlayerEntity plr = game.plrs.GetPlayer(plr_defending);
        if (plr.Board.Count == 0) EndTurn(null);

        bool can_play_a_card = false;
        for (int i = 0; i < plr.Hand.Count; i++) {
            if (CanPlayCard(plr.Hand[i])) {
                can_play_a_card = true;
                break;
            }
        }

        if (!can_play_a_card) EndTurn(null);
    }

    public void StartState() { }

    public bool CanPlayCard(CardEntity card) {
        PlayerEntity plr = game.plrs.GetPlayer(plr_defending);
        if (plr_defending != card.PlrId) return false;
        if (defending_units != 0) return false;
        if (card.Type != CardTypes.FastSpell) return false;
        if (card.Stats.Cost > plr.Mana) return false;
        if (plr.Hand.Contains(card)) return false;

        return true;
    }

    public void EndTurn(EndTurnRequest? req) {
        if (req != null && req.PlayerId != plr_defending) return;
        HandleAttackPhase();
        game.SetGameState(new RegularState(game, true));
    }

    public void ToggleDefend(ToggleDefendRequest data) {
        if (data.UnitDefending == -1) {
            CancelDefend(data);
            return;
        }
        if (plr_defending == data.PlayerId) return; //if ur the defender

        PlayerEntity plr = game.plrs.GetOtherPlayer(plr_defending);
        if (!plr.Board.Contains(game.cards.GetCard(data.UnitDefending))) return; //check the defending unit is on board
        if (!attacking_units.ContainsKey(data.UnitAttacking)) return; //the specified unit getting defended is not attacking

        //if already defending something, then no
        foreach (KeyValuePair<int, HashSet<int>> pair in attacking_units) {
            if (pair.Value.Remove(data.UnitDefending)) {
                defending_units--;
                break;
            }
        }

        defending_units++;

        attacking_units[data.UnitAttacking].Add(data.UnitDefending);
    }

    public void CancelDefend(ToggleDefendRequest data) {
        if (game.Plr_Turn == data.PlayerId) return; //if ur the defender

        if (!attacking_units[data.UnitAttacking].Contains(data.UnitDefending)) return; //the unit is not defending the attacking unit

        defending_units--;

        attacking_units[data.UnitAttacking].Remove(data.UnitDefending);
    }

    public void HandleAttackPhase() {
        //make cards attack players too
        game.MakeCounterableEffect(
            plr_defending,
            null,
            () => {
                foreach (KeyValuePair<int, HashSet<int>> pair in attacking_units) {
                    AttackEnemies(game.cards.GetCard(pair.Key), [.. pair.Value.Select(game.cards.GetCard)]);
                }
            }
        );
    }

    public void AttackEnemies(CardEntity card, List<CardEntity> victims) {
        int atk = card.Stats.Attack;

        if (victims.Count == 0) {
            game.plrs.GetOtherPlayer(card.PlrId).ChangeHealth(-atk);
            return;
        }

        foreach (CardEntity victim in victims) {
            atk = card.AttackCard(victim, atk);
        }
    }
}