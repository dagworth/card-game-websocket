namespace server.GameLogic;

using shared.DTOs;

using System.Text.Json.Serialization;

public class CardStats {
    [JsonPropertyName("cost")] public int Cost { get; set; }
    [JsonPropertyName("health")] public int Health { get; set; }
    [JsonPropertyName("damaged")] public int Damaged { get; set; }
    [JsonPropertyName("attack")] public int Attack { get; set; }
    [JsonPropertyName("passives")] public List<Passives> passives { get; set; }

    public CardStats(CardDataDTO data) {
        Cost = data.cost;
        Health = data.health;
        Attack = data.attack;
        passives = [.. data.passives];
    }

    public CardStats() {
        passives = [];
    }
}