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

public abstract class ClientRequest(int playerid) {
    [JsonPropertyName("player_id")] public int PlayerId { get; set; } = playerid;
}

public class JoinQueueRequest(int playerid) : ClientRequest(playerid);
public class EndTurnRequest(int playerid) : ClientRequest(playerid);

public class PlayCardRequest(int playerid, int cardid, List<int> targets) : ClientRequest(playerid) {
    [JsonPropertyName("card_id")] public int CardId { get; set; } = cardid;
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = targets;
}

public class ToggleAttackRequest(int playerid, int unitattacking) : ClientRequest(playerid) {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; } = unitattacking;
}

public class ToggleDefendRequest(int playerid, int unitattacking, int unitdefending) : ClientRequest(playerid) {
    [JsonPropertyName("unit_attacking")] public int UnitAttacking { get; set; } = unitattacking;
    [JsonPropertyName("unit_defending")] public int UnitDefending { get; set; } = unitdefending;
}

public class TargetsChoiceRequest(int playerid, List<int> targets) : ClientRequest(playerid) {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = targets;
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TargetOptions), "targetoptions")]
[JsonDerivedType(typeof(InformId), "informid")]
[JsonDerivedType(typeof(GameUpdate), "gameupdate")]

public abstract class ServerEvent { }

public class TargetOptions(List<int> targets) : ServerEvent {
    [JsonPropertyName("targets")] public List<int> Targets { get; set; } = targets;
}

public class InformId(int playerid) : ServerEvent {
    [JsonPropertyName("player_id")] public int PlayerId { get; set; } = playerid;
}

public class GameUpdate(List<ClientUpdater> events) : ServerEvent {
    [JsonPropertyName("events")] public List<ClientUpdater> Events { get; set; } = events;
}