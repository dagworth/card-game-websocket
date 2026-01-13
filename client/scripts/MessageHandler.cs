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
        ServerToClientMessage data = JsonSerializer.Deserialize<ServerToClientMessage>(message);
        if (data.Action.Equals("informid")) {
            InformId a = JsonSerializer.Deserialize<InformId>(message);
            return a.Id;
        }

        if (data.Action.Equals("choosetargets")) {
            TargetOptions a = JsonSerializer.Deserialize<TargetOptions>(message);
            List<int> targets = a.Targets; //just for reference
        }

        if (data.Action.Equals("clientupdate")) {
            ClientUpdateMessage a = JsonSerializer.Deserialize<ClientUpdateMessage>(message);
            foreach (ClientUpdater updater in a.Events) {
                if(updater is CardLocationUpdater cardlocaation) {
                    
                } else if (updater is StatUpdater stat) {

                } else if (updater is NewCardUpdater newcard) {
                    GD.Print($"got card {newcard.card.Name}");
                    // UIController.addCard(1);
                } else if (updater is DamageUpdater damage) {

                } else if (updater is TurnUpdater turn) {

                }
            }
        }




        return 0; //never should get here
    }

    public static void SendEndTurn() {
        ClientToServerMessage clone = new();
        clone.Action = "end_turn";
        clone.PlayerId = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize(clone));
    }

    public static void SendJoinQueue() {
        ClientToServerMessage clone = new();
        clone.Action = "join_waiting_queue";
        clone.PlayerId = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize(clone));
    }
}