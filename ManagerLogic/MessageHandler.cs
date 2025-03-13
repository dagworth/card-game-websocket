using Fleck;
using System.Text.Json;

public static class MessageHandler {
    public static void ReadMessage(IWebSocketConnection ws, string message, string action) {

        switch(action){
            case "play_card":
                GameActionExecutor.ExecutePlayCard(JsonSerializer.Deserialize<PlayCard>(message)!);
                break;
            case "units_attack":
                GameActionExecutor.ExecuteUnitsAttack(JsonSerializer.Deserialize<UnitsAttack>(message)!);
                break;
            case "end_turn":
                GameActionExecutor.ExecuteEndTurn(JsonSerializer.Deserialize<EndTurn>(message)!);
                break;
            case "join_waiting_queue":
                ServerHandler.AddPlayerToQueue(ws);
                break;
            default:
                Console.WriteLine($"this message didn't have a valid action {action}");
                break;
        }
    }
}