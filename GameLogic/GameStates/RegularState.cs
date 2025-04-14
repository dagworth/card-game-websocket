public class RegularState(Game game, bool attacked) : IGameState {
    private readonly Game game = game;
    private bool attacked = attacked;

    public void StartState(){}

    public void EndTurn(){
        game.ChangeTurn();
    }
    
    public bool CanPlayCard(CardStatus card){
        Player plr = game.plrs.GetPlayer(game.Plr_Turn);
        if(game.Plr_Turn != card.Plr_Id) return false;
        if(card.Cost > plr.Mana) return false;
        if(plr.FindIndex(plr.Hand, card.Id) == -1) return false;

        return true;
    }

    public void ToogleAttack(ToggleAttack data){
        if(attacked) return;
        AttackingState a = new AttackingState(game);
        game.SetGameState(a);
        a.ToogleAttack(data);
    }
}