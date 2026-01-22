namespace shared.DTOs;

using System.Text.Json.Serialization;

public class CardEntityDTO(int id, int plrid, CardTypes type, CardLocations location, string name, CardStatsDTO stats) {
    [JsonPropertyName("card_id")] public int Id { get; private set; } = id;
    [JsonPropertyName("plr_id")] public int PlrId { get; private set; } = plrid;
    [JsonPropertyName("type")] public CardTypes Type { get; private set; } = type;
    [JsonPropertyName("location")] public CardLocations Location { get; private set; } = location;
    [JsonPropertyName("name")] public string Name { get; private set; } = name;
    [JsonPropertyName("stats")] public CardStatsDTO Stats { get; private set; } = stats;
}