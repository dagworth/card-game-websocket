using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

public class Message {
    [JsonPropertyName("action")] public string Action = "";
    [JsonPropertyName("player_id")] public int PlayerId = 0;
}

public class PlayCard : Message {
    [JsonPropertyName("game_id")] public int GameId = 0;
    [JsonPropertyName("card_id")] public int CardId = 0;
    [JsonPropertyName("target_type")] public CardTypes TargetType = CardTypes.Minion;
    [JsonPropertyName("target_id")] public int TargetId = 0;
}

public class MinionsAttack : Message {
    [JsonPropertyName("game_id")] public int GameId = 0;
    [JsonPropertyName("minions_attacking")] public List<int> MinionsAttacking = [];
}

public class EndTurn : Message {
    [JsonPropertyName("game_id")] public int GameId = 0;
}
