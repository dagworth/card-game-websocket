using System.Text.Json;

public static class GameActionExecutor {
    public static void ExecutePlayCard(PlayCard data){
        Player plr = GameManager.GetPlayer(data.PlayerId);
        Game game = plr.GetGame();

        if(game.CanPlayCard(data.PlayerId, data.CardId)){
            plr.PlayCard(data.CardId, data.Targets);
        }
    }

    public static void ExecuteUnitsAttack(UnitsAttack data){

    }

    public static void ExecuteEndTurn(EndTurn data){

    }
}