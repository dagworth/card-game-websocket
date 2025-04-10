using Fleck;
using System.Text.Json;

public static class MessageHandler {
    public static void ReadMessage(IWebSocketConnection ws, int plr_id, string message, string action) {
        //this is very temporary, i need to make it so the server wont crash cus of this
        //we r trusting client requests arent stupid for now
        
        if(action.Equals("join_waiting_queue")){
            ServerHandler.AddPlayerToQueue(ws);
            return;
        }

        Game game = GameManager.GetPlayer(plr_id).Game;

        if(action.Equals("play_card")){
            PlayCard data = JsonSerializer.Deserialize<PlayCard>(message)!;
            game.PlayerPlayCard(data);
        }

        else if(action.Equals("toggle_atk")){
            ToggleAttack data = JsonSerializer.Deserialize<ToggleAttack>(message)!;
            if(game.GetGameState() is AttackingState a){
                a.ToogleAttack(data);
            } else if(game.GetGameState() is RegularState b){
                b.ToogleAttack(data);
            }
        }

        else if(action.Equals("cancel_atk")){
            ToggleAttack data = JsonSerializer.Deserialize<ToggleAttack>(message)!;
            if(game.GetGameState() is AttackingState a){
                a.CancelAttack(data);
            }
        }

        else if(action.Equals("toggle_def")){
            ToggleDefend data = JsonSerializer.Deserialize<ToggleDefend>(message)!;
            if(game.GetGameState() is DefendingState a){
                a.ToggleDefend(data);
            }
        }

        else if(action.Equals("cancel_def")){
            ToggleDefend data = JsonSerializer.Deserialize<ToggleDefend>(message)!;
            if(game.GetGameState() is DefendingState a){
                a.CancelDefend(data);
            }
        }

        else if(action.Equals("end_turn")){
            Message data = JsonSerializer.Deserialize<Message>(message)!;
            game.GetGameState().EndTurn();
        }

        else if(action.Equals("targets_choice")){
            TargetsChoice data = JsonSerializer.Deserialize<TargetsChoice>(message)!;
            if(game.GetGameState() is ChoosingState a){
                a.GotTargets(data);
            }
        }
    }

    public static void AskForTargets(IWebSocketConnection ws, List<int> message){
        TargetsChoice a = new(){
            Targets = message
        };

        ws.Send(JsonSerializer.Serialize(a));
    }
}