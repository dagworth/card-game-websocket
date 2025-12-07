using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Message {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}

public class PlayCard : Message {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ToggleAttack : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefend : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class TargetsChoice : Message {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ClientUpdateMessage(List<ClientUpdater> events) : Message {
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; } = events;
}