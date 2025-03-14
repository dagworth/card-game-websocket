using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Message {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}
public class PlayCard : Message {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<PlayerTargets> Targets { get; set; } = [];
}

public class PlayerTargets {
    [JsonPropertyName("target_type")] public CardTypes TargetType { get; set; }
    [JsonPropertyName("target_id")] public int TargetId { get; set; }
}

public class ToggleAttack : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ReverseToggleAttack : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefend : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class ReverseToggleDefend : Message {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}
