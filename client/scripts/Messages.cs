using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Collections.Generic;

public class RecievedMessage {
    public string action { get; set; }
}

public class SendMessage {
    public string action { get; set; }
    [JsonPropertyName("player_id")] public int Id { get; set; }
}

public class PlayCard : SendMessage {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ToggleAttack : SendMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefend : SendMessage {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class TargetsChoice : SendMessage {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class TargetOptions {
    public string action { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; }
}

public class InformId {
    public string action { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
}

public class ClientUpdateMessage {
    public string action { get; set; }
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; }
}