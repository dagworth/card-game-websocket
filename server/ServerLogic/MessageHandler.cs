namespace server.ServerLogic;

using server.GameLogic.Entities;
using server.GameLogic.GameStates;
using shared;

using Fleck;
using System.Text.Json;

public static class MessageHandler {
    public static void ReadMessage(IWebSocketConnection ws, int plr_id, string message) {
        //this is very temporary, i need to make it so the server wont crash cus of this
        //we r trusting client requests arent stupid for now
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        ClientRequest request = JsonSerializer.Deserialize<ClientRequest>(message,options)!;

        if (request is JoinQueueRequest) {
            ServerHandler.AddPlayerToQueue(ws);
            return;
        }

        GameEntity game = GameManager.GetPlayer(plr_id).Game;

        switch (request) {
            case PlayCardRequest req:
                game.PlayerPlayCard(req);
                break;

            case ToggleAttackRequest req:
                if (game.Game_State is AttackingState a) a.ToogleAttack(req);
                else if (game.Game_State is RegularState b) b.ToogleAttack(req);
                break;

            case ToggleDefendRequest req:
                if (game.Game_State is DefendingState c) c.ToggleDefend(req);
                break;

            case EndTurnRequest:
                game.PlayerEndTurn();
                break;

            case TargetsChoiceRequest req:
                if (game.Game_State is ChoosingState d) d.GotTargets(req);
                break;
        }
    }

    public static void AskForTargets(IWebSocketConnection ws, List<int> message){
        TargetOptions a = new(){
            Targets = message
        };

        ws.Send(JsonSerializer.Serialize<ServerEvent>(a));
    }

    public static void UpdateClient(IWebSocketConnection ws, GameUpdate message)
    {
        ws.Send(JsonSerializer.Serialize<ServerEvent>(message));
    }
}