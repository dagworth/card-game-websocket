namespace shared;

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class ServerToClientMessage {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
}

public class ClientToServerMessage {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}

public class PlayCard : ClientToServerMessage {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ToggleAttack : ClientToServerMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefend : ClientToServerMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class TargetsChoice : ClientToServerMessage {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class TargetOptions : ServerToClientMessage {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class InformId : ServerToClientMessage {
    [JsonPropertyName("id")] public int Id { get; set; }
}

public class ClientUpdateMessage : ServerToClientMessage {
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; } = [];
}