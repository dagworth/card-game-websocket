using System.Text.Json.Serialization;
using System.Collections.Generic;

public class CardStats {
    [JsonPropertyName("cost")] public int Cost { get; set; }
    [JsonPropertyName("health")] public int Health { get; set; }
    [JsonPropertyName("damaged")] public int Damaged { get; set; }
    [JsonPropertyName("attack")] public int Attack { get; set; }
    [JsonPropertyName("passives")] public List<Passives> passives { get; set; }
}