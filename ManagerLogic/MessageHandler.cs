using Fleck;
using System.Text.Json;

public static class MessageHandler {
    public static void ReadMessage(IWebSocketConnection ws, int plr_id, string message, string action) {
        //this is very temporary, i need to make it so the server wont crash cus of this
        //we r trusting client requests arent stupid for now
        switch(action){
            case "play_card":
                GameManager.GetPlayer(plr_id).GetGame().ExecutePlayCard(JsonSerializer.Deserialize<PlayCard>(message)!);
                break;
            case "toggle_attack":
                GameManager.GetPlayer(plr_id).GetGame().ExecuteToggleAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                break;
            case "toggle_defend":
                GameManager.GetPlayer(plr_id).GetGame().ExecuteToggleDefend(JsonSerializer.Deserialize<ToggleDefend>(message)!);
                break;
            case "reverse_toggle_attack":
                GameManager.GetPlayer(plr_id).GetGame().ExecuteReverseToggleAttack(JsonSerializer.Deserialize<ReverseToggleAttack>(message)!);
                break;
            case "reverse_toggle_defend":
                GameManager.GetPlayer(plr_id).GetGame().ExecuteReverseToggleDefend(JsonSerializer.Deserialize<ReverseToggleDefend>(message)!);
                break;
            case "end_turn":
                GameManager.GetPlayer(plr_id).GetGame().ExecuteEndTurn(JsonSerializer.Deserialize<Message>(message)!);
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