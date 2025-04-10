public class RegularState(Game game) : IGameState {
    private Game game = game;

    public void StateStarted(){
        
    }
    
    public bool CanPlayCard(CardStatus card){
        Player plr = game.GetPlayer(game.Plr_Turn);
        if(game.Plr_Turn != card.plr_id) return false;
        if(card.cost > plr.Mana) return false;
        if(plr.FindIndex(plr.Hand, card.card_id) == -1) return false;

        return true;
    }

    public void EndTurn(){
        game.ChangeTurn();
    }

    public void ToogleAttack(ToggleAttack data){
        AttackingState a = new AttackingState(game);
        game.SetGameState(a);
        a.ToogleAttack(data);
    }
}