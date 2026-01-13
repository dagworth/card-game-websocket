using Godot;
using System.Text.Json.Serialization;

using shared.DTOs;

public class CardEntity {
    [JsonPropertyName("card_id")] public int Id { get; private set; }
    [JsonPropertyName("type")] public CardTypes Type { get; private set; }
    [JsonPropertyName("location")] public CardLocations Location { get; private set; }
    [JsonPropertyName("name")] public string Name { get; private set; }
    [JsonPropertyName("stats")] public CardStatsDTO Stats { get; private set; }
}