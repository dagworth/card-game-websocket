using System.Text.Json;

public static class GameActionExecutor {
    public static void ExecutePlayCard(PlayCard data){
        Game game = GameManager.GetGame(data.PlayerId);
        CardStatus card = game.GetCard(data.CardId);
        CardTypes type = card.type;
        switch(type){
            case CardTypes.Minion:
                
                break;
            default:
                Console.WriteLine("bro forgot to code the cardtype in");
                break;
        }
        if(game.IsPlayerTurn(data.PlayerId)){
            Player plr = game.GetPlayer(data.PlayerId);
            plr.PlayCard(data.CardId, data.TargetType, data.TargetId);
        }
    }

    public static void ExecuteMinionsAttack(MinionsAttack data){
        Game game = GameManager.GetGame(data.PlayerId);

    }

    public static void ExecuteEndTurn(EndTurn data){
        Game game = GameManager.GetGame(data.PlayerId);

    }
}