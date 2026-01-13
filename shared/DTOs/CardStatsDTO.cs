namespace shared.DTOs;

using System.Text.Json.Serialization;

public class CardStatsDTO(int cost, int health, int damaged, int attack, List<Passives> passives) {
    [JsonPropertyName("cost")] public int Cost { get; set; } = cost;
    [JsonPropertyName("health")] public int Health { get; set; } = health;
    [JsonPropertyName("damaged")] public int Damaged { get; set; } = damaged;
    [JsonPropertyName("attack")] public int Attack { get; set; } = attack;
    [JsonPropertyName("passives")] public List<Passives> passives { get; set; } = passives;
}