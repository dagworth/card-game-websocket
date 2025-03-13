using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Message {
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}
public class PlayCard : Message {
    [JsonPropertyName("game_id")] public int GameId { get; set; }
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<PlayerTargets> Targets { get; set; } = [];
}

public class PlayerTargets {
    [JsonPropertyName("target_type")] public CardTypes TargetType { get; set; }
    [JsonPropertyName("target_id")] public int TargetId { get; set; }
}

public class UnitsAttack : Message {
    [JsonPropertyName("game_id")] public int GameId { get; set; }
    [JsonPropertyName("units_attacking")] public List<int> UnitsAttacking { get; set; } = [];
}

public class EndTurn : Message {
    [JsonPropertyName("game_id")] public int GameId { get; set; }
}
