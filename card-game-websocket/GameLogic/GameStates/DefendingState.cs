public class DefendingState : IGameState {
    private readonly GameEntity game;
    private readonly int plr_defending;
    private readonly Dictionary<int, List<int>> attacking_units;
    private int defending_units = 0;

    public DefendingState(GameEntity game, Dictionary<int, List<int>> attacking_units) {
        this.game = game;
        plr_defending = game.Plr_Turn;
        this.attacking_units = attacking_units;

        PlayerEntity plr = game.plrs.GetPlayer(plr_defending);
        if (plr.Board.Count == 0) EndTurn();

        bool can_play_a_card = false;
        for (int i = 0; i < plr.Hand.Count; i++) {
            if (CanPlayCard(plr.Hand[i])) {
                can_play_a_card = true;
                break;
            }
        }

        if (!can_play_a_card) EndTurn();
    }

    public void StartState() { }

    public bool CanPlayCard(CardEntity card) {
        PlayerEntity plr = game.plrs.GetPlayer(plr_defending);
        if (plr_defending != card.Plr_Id) return false;
        if (defending_units != 0) return false;
        if (card.Type != CardTypes.FastSpell) return false;
        if (card.Stats.Cost > plr.Mana) return false;
        if (plr.Hand.Contains(card)) return false;

        return true;
    }

    public void EndTurn() {
        HandleAttackPhase();
        game.SetGameState(new RegularState(game, true));
    }

    public void ToggleDefend(ToggleDefend data) {
        if (plr_defending == data.PlayerId) return; //if ur the defender

        PlayerEntity plr = game.plrs.GetOtherPlayer(plr_defending);
        if (!plr.Board.Contains(game.cards.GetCard(data.UnitDefending))) return; //check the unit is on board
        if (!attacking_units.ContainsKey(data.UnitAttacking)) return; //the specified unit getting defended is not attacking

        defending_units++;

        //check if unit is already defending anything
        foreach (KeyValuePair<int, List<int>> pair in attacking_units) {
            if (pair.Value.Contains(data.UnitDefending)) return;
        }

        attacking_units[data.UnitAttacking].Add(data.UnitDefending);
    }

    public void CancelDefend(ToggleDefend data) {
        if (game.Plr_Turn == data.PlayerId) return; //if ur the defender

        if (!attacking_units[data.UnitAttacking].Contains(data.UnitDefending)) return; //the unit is not defending the attacking unit

        defending_units--;

        attacking_units[data.UnitAttacking].Remove(data.UnitDefending);
    }

    public void HandleAttackPhase() {
        game.MakeCounterableEffect(
            plr_defending,
            null,
            () => {
                foreach (KeyValuePair<int, List<int>> pair in attacking_units) {
                    AttackEnemies(game.cards.GetCard(pair.Key), [.. pair.Value.Select(game.cards.GetCard)]);
                }
            }
        );
    }

    public void AttackEnemies(CardEntity card, List<CardEntity> victims) {
        int atk = card.Stats.Attack;

        if (victims.Count == 0) {
            game.plrs.GetOtherPlayer(card.Plr_Id).ChangeHealth(-atk);
            return;
        }

        foreach (CardEntity victim in victims) {
            atk = card.AttackCard(victim, atk);
        }
    }
}