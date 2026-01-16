namespace shared.DTOs;

using System.Text.Json.Serialization;

public class CardDataDTO {
    public string Name {get; set; } = "";
    public CardTypes Type {get; set; }
    public string Description {get; set; } = "";
    public int Cost {get; set; }
    public int Health {get; set; }
    public int Attack {get; set; }

    public List<Tribes> Tribes {get; set; } = [];
    public List<Passives> Passives {get; set; } = [];
}