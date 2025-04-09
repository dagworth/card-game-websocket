using Fleck;
using System.Text.Json;

public static class MessageHandler {
    public static void ReadMessage(IWebSocketConnection ws, int plr_id, string message, string action) {
        //this is very temporary, i need to make it so the server wont crash cus of this
        //we r trusting client requests arent stupid for now
        switch(action){
            case "play_card":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecutePlayCard(JsonSerializer.Deserialize<PlayCard>(message)!);
                break;
            case "toggle_atk":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteToggleAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                break;
            case "toggle_def":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteToggleDefend(JsonSerializer.Deserialize<ToggleDefend>(message)!);
                break;
            case "r_toggle_atk":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteReverseToggleAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                break;
            case "r_toggle_def":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteReverseToggleDefend(JsonSerializer.Deserialize<ToggleDefend>(message)!);
                break;
            case "end_turn":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteEndTurn(JsonSerializer.Deserialize<Message>(message)!);
                break;
            case "targets_choice":
                GameManager.GetPlayer(plr_id).Game.messageActions.ExecuteTargetsChoice(JsonSerializer.Deserialize<TargetsChoice>(message)!);
                break;
            case "join_waiting_queue":
                ServerHandler.AddPlayerToQueue(ws);
                break;
            default:
                Console.WriteLine($"this message didn't have a valid action {action}");
                break;
        }
    }

    public static void AskForTargets(IWebSocketConnection ws, List<int> message){
        TargetsChoice a = new(){
            Targets = message
        };

        ws.Send(JsonSerializer.Serialize(a));
    }
}