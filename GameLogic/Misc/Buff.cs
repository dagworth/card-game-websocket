using System.Text.Json.Serialization;

public class Buff {
    public CardEntity? card;
    [JsonPropertyName("cost")] public int Cost { get; set; }
    [JsonPropertyName("cost_fixed")] public int Cost_Fixed { get; set; }
    [JsonPropertyName("attack")] public int Attack { get; set; }
    [JsonPropertyName("attack_fixed")] public int Attack_Fixed { get; set; }
    [JsonPropertyName("health")] public int Health { get; set; }
    [JsonPropertyName("health_fixed")] public int Health_Fixed { get; set; }
    [JsonPropertyName("passives")] public List<Passives> passives { get; set; } = [];

    public void RemoveBuff(){
        if(card == null){
            Console.WriteLine("you fucked up (buff was attached to nobody)");
        }
        card!.RemoveTempBuff(this);
    }
}