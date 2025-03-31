public class ActionExecutor(Game game) {
    private readonly Game game = game;

    //this function needs way more work cus it doesnt support like
    //fast spells and ambush
    //i also need to figure out how to implement player choice targetting ex. "give a unit +1/+1"
    public void ExecutePlayCard(PlayCard data){
        CardStatus card = game.GetCard(data.CardId);
        Player plr = game.GetPlayer(data.PlayerId);
        if(plr.mana < card.cost) return;

        if(game.plr_turn != data.PlayerId){
            if(game.plr_action_priority == data.PlayerId){
                plr.PlayCard(data.CardId);
            }
        } else {
            plr.PlayCard(data.CardId);
        }
    }

    public void ExecuteEndTurn(Message data){
        switch(game.game_state){
            case GameState.Attacking:
                if(data.PlayerId == game.plr_turn){
                    game.game_state = GameState.Defending;
                }
                break;
            case GameState.Defending:
                if(data.PlayerId != game.plr_turn){
                    game.HandleAttackPhase();
                    game.GetPlayer(game.plr_turn).Attacked();
                    game.game_state = GameState.RegularTurn;
                }
                break;
            case GameState.RegularTurn:
                if(data.PlayerId == game.plr_turn){
                    game.EndTurn();
                }
                break;
            case GameState.PriorityTurn:
                break;
        }
    }

    public void ExecuteToggleAttack(ToggleAttack data){
        if(game.plr_turn != data.PlayerId) return; //if ur the attacker, can only attack on your turn
        if(game.GetPlayer(data.PlayerId).attacked) return; //if you attacked already
        if(!(game.game_state is GameState.Attacking or GameState.RegularTurn)) return; //check the right gamestate

        Player plr = game.GetPlayer(game.plr_turn);
        if(plr.FindIndex(plr.Board, data.UnitAttacking) == -1) return; //check the unit is on board

        game.game_state = GameState.Attacking;
        game.attacking_units[data.UnitAttacking] = [];
    }

    public void ExecuteReverseToggleAttack(ReverseToggleAttack data){
        if(game.plr_turn != data.PlayerId) return; //if ur the attacker
        if(game.game_state is not GameState.Attacking) return; //check the right gamestate

        game.attacking_units.Remove(data.UnitAttacking);
        if(game.attacking_units.Count == 0){
            game.game_state = GameState.RegularTurn;
        }
    }

    public void ExecuteToggleDefend(ToggleDefend data){
        if(game.plr_turn == data.PlayerId) return; //if ur the defender
        if(game.game_state is not GameState.Defending) return; //check the right gamestate

        Player plr = game.GetOtherPlayer(game.plr_turn);
        if(plr.FindIndex(plr.Board, data.UnitDefending) == -1) return; //check the unit is on board
        if(!game.attacking_units.ContainsKey(data.UnitAttacking)) return; //the specified unit getting defended is not attacking

        //check if unit is already defending anything
        foreach (KeyValuePair<int,List<int>> pair in game.attacking_units){
            if(pair.Value.Contains(data.UnitDefending)) return;
        }
        
        game.attacking_units[data.UnitAttacking].Add(data.UnitDefending);
    }

    public void ExecuteReverseToggleDefend(ReverseToggleDefend data){
        if(game.plr_turn == data.PlayerId) return; //if ur the defender
        if(game.game_state is not GameState.Defending) return; //check the right gamestate

        if(!game.attacking_units[data.UnitAttacking].Contains(data.UnitDefending)) return; //the unit is not defending the attacking unit
        
        game.attacking_units[data.UnitAttacking].Remove(data.UnitDefending);
    }
}