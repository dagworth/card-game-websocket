using Godot;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;

using shared;
using System.Threading;

public partial class MessageHandler : Node {
    //returns int if its the id and only then
    public static int ExecuteMessage(string message) {
        GD.Print(message);
        ServerEvent data = JsonSerializer.Deserialize<ServerEvent>(message);
        if (data is InformId a) {
            return a.PlayerId;
        }

        if (data is TargetOptions b) {
            List<int> targets = b.Targets; //just for reference
        }

        if (data is GameUpdate c) {
            foreach (ClientUpdater updater in c.Events) {
                if(updater is CardLocationUpdater cardlocaation) {
                    
                } else if (updater is StatUpdater stat) {

                } else if (updater is NewCardUpdater newcard) {
                    GD.Print($"got card {newcard.card.Name}");
                    CardHandler.AddCard(newcard.card);
                } else if (updater is DamageUpdater damage) {

                } else if (updater is TurnUpdater turn) {

                }
            }
        }




        return 0; //never should get here
    }

    public static void SendEndTurn() {
        EndTurnRequest clone = new();
        clone.PlayerId = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
    }

    public static void SendJoinQueue() {
        JoinQueueRequest clone = new();
        clone.PlayerId = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize<ClientRequest>(clone));
    }
}