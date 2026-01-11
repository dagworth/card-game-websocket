using Godot;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;

public partial class MessageHandler : Node {
    //returns int if its the id and only then
    public static int ExecuteMessage(string message) {
        GD.Print($"trying to execute {message}");
        RecievedMessage data = JsonSerializer.Deserialize<RecievedMessage>(message);
        if (data.action.Equals("informid")) {
            InformId a = JsonSerializer.Deserialize<InformId>(message);
            return a.Id;
        }

        if (data.action.Equals("choosetargets")) {
            TargetOptions a = JsonSerializer.Deserialize<TargetOptions>(message);
            List<int> targets = a.Targets; //just for reference
        }

        if (data.action.Equals("clientupdate")) {
            ClientUpdateMessage a = JsonSerializer.Deserialize<ClientUpdateMessage>(message);
            foreach (ClientUpdater updater in a.Events) {
                GD.Print(updater.Action);
            }
        }




        return 0; //never should get here
    }

    public static void SendEndTurn() {
        SendMessage clone = new();
        clone.action = "end_turn";
        clone.Id = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize(clone));
    }

    public static void SendJoinQueue() {
        SendMessage clone = new();
        clone.action = "join_waiting_queue";
        clone.Id = ClientHandler.plr_id;
        ClientHandler.SendMessage(JsonSerializer.Serialize(clone));
    }
}