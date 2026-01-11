using System.Text.Json.Serialization;
using System.Collections.Generic;

public class Buff {
    [JsonPropertyName("cost")] public int Cost { get; set; }
    [JsonPropertyName("cost_fixed")] public int Cost_Fixed { get; set; }
    [JsonPropertyName("attack")] public int Attack { get; set; }
    [JsonPropertyName("attack_fixed")] public int Attack_Fixed { get; set; }
    [JsonPropertyName("health")] public int Health { get; set; }
    [JsonPropertyName("health_fixed")] public int Health_Fixed { get; set; }
    [JsonPropertyName("passives")] public List<Passives> passives { get; set; } = [];
}