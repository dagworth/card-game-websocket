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

        GameEntity game = GameManager.GetPlayer(plr_id).Game;

        if(action.Equals("play_card")){
            game.PlayerPlayCard(JsonSerializer.Deserialize<PlayCard>(message)!);
            //update for both players
        }

        else if(action.Equals("toggle_atk")){
            if(game.Game_State is AttackingState a){
                a.ToogleAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                //update for other player
            } else if(game.Game_State is RegularState b){
                b.ToogleAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                //update for other player
            }
        }

        else if(action.Equals("cancel_atk")){
            if(game.Game_State is AttackingState a){
                a.CancelAttack(JsonSerializer.Deserialize<ToggleAttack>(message)!);
                //update for other player
            }
        }

        else if(action.Equals("toggle_def")){
            if(game.Game_State is DefendingState a){
                a.ToggleDefend(JsonSerializer.Deserialize<ToggleDefend>(message)!);
                //update for other player
            }
        }

        else if(action.Equals("cancel_def")){
            if(game.Game_State is DefendingState a){
                a.CancelDefend(JsonSerializer.Deserialize<ToggleDefend>(message)!);
                //update for other player
            }
        }

        else if(action.Equals("end_turn")){
            game.PlayerEndTurn();
            //update for both players
        }

        else if(action.Equals("targets_choice")){
            if(game.Game_State is ChoosingState a){
                a.GotTargets(JsonSerializer.Deserialize<TargetsChoice>(message)!);
                //update for both players
            }
        }
    }

    public static void AskForTargets(IWebSocketConnection ws, List<int> message){
        TargetsChoice a = new(){
            Targets = message
        };

        ws.Send(JsonSerializer.Serialize(a));
    }

    public static void UpdateClient(IWebSocketConnection ws, ClientUpdateMessage message)
    {
        ws.Send(JsonSerializer.Serialize(message));
    }
}