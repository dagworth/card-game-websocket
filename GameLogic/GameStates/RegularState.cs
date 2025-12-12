public class RegularState(GameEntity game, bool attacked) : IGameState {
    private readonly GameEntity game = game;
    private bool attacked = attacked;

    public void StartState()
    {
        
    }

    public void EndTurn(){
        game.updater.EndTurn(game.Plr_Turn);
        game.delayed.DoEndTurnEffects(game.Plr_Turn);
        game.updater.UpdateClients("end turn");
        game.MakeCounterableEffect(
            game.Plr_Turn,
            null,
            () => {}
        );
        game.ChangeTurn();
        game.delayed.DoStartTurnEffects(game.Plr_Turn);
        game.updater.UpdateClients("start turn");
    }
    
    public bool CanPlayCard(CardEntity card){
        PlayerEntity plr = game.plrs.GetPlayer(game.Plr_Turn);
        if(!plr.Hand.Contains(card)) return false;
        if(game.Plr_Turn != card.Plr_Id) return false;
        if(card.Stats.Cost > plr.Mana) return false;

        return true;
    }

    public void ToogleAttack(ToggleAttack data){
        if(attacked) return;
        AttackingState a = new AttackingState(game);
        game.SetGameState(a);
        a.ToogleAttack(data);
    }
}