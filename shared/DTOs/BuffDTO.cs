namespace shared.DTOs;

using System.Text.Json.Serialization;
using System.Collections.Generic;

public class BuffDTO(int cost, int cost_fixed, int attack, int attack_fixed, int health, int health_fixed, List<Passives> passives) {
    [JsonPropertyName("cost")] public int Cost { get; private set; } = cost;
    [JsonPropertyName("cost_fixed")] public int Cost_Fixed { get; private set; } = cost_fixed;
    [JsonPropertyName("attack")] public int Attack { get; private set; } = attack;
    [JsonPropertyName("attack_fixed")] public int Attack_Fixed { get; private set; } = attack_fixed;
    [JsonPropertyName("health")] public int Health { get; private set; } = health;
    [JsonPropertyName("health_fixed")] public int Health_Fixed { get; private set; } = health_fixed;
    [JsonPropertyName("passives")] public List<Passives> passives { get; private set; } = passives;
}