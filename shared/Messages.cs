namespace shared;

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(JoinQueueRequest), "joinqueue")]
[JsonDerivedType(typeof(EndTurnRequest), "endturn")]
[JsonDerivedType(typeof(PlayCardRequest), "playcard")]
[JsonDerivedType(typeof(ToggleAttackRequest), "toggleattack")]
[JsonDerivedType(typeof(ToggleDefendRequest), "toggledefend")]
[JsonDerivedType(typeof(TargetsChoiceRequest), "targetschoice")]

public class ClientRequest {
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}

public class JoinQueueRequest : ClientRequest { }
public class EndTurnRequest : ClientRequest { }

public class PlayCardRequest : ClientRequest {
    [JsonPropertyName("card_id")] public int CardId { get; set; }
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class ToggleAttackRequest : ClientRequest {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
}

public class ToggleDefendRequest : ClientRequest {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; }
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; }
}

public class TargetsChoiceRequest : ClientRequest {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TargetOptions), "targetoptions")]
[JsonDerivedType(typeof(InformId), "informid")]
[JsonDerivedType(typeof(GameUpdate), "gameupdate")]

public class ServerEvent { }

public class TargetOptions : ServerEvent {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = [];
}

public class InformId : ServerEvent {
    [JsonPropertyName("player_id")] public int PlayerId { get; set; }
}

public class GameUpdate : ServerEvent {
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; } = [];
}