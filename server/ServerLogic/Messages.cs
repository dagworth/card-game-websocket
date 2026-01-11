using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class RecievedMessage {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}

public class PlayCard : RecievedMessage {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ToggleAttack : RecievedMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefend : RecievedMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class TargetsChoice : RecievedMessage {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class TargetOptions {
    public string action { get; set; } = "choosetargets";
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class InformId {
    public string action { get; set; } = "informid";
    [JsonPropertyName("id")] public int Id { get; set; }
}

public class ClientUpdateMessage {
    public string action { get; set; } = "clientupdate";
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; } = [];
}