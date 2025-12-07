using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[JsonDerivedType(typeof(CardLocationUpdater), "CLU")]
[JsonDerivedType(typeof(StatUpdater), "SU")]

public class ClientUpdater
{
    [JsonPropertyName("action")] public string Action { get; set; } = "";
}

public class CardLocationUpdater(CardLocations new_loc, CardLocations starting_loc, int card_id) : ClientUpdater
{
    [JsonPropertyName("card_id")] public int Card_Id { get; set; } = card_id;
    [JsonPropertyName("now")] public CardLocations Now { get; set; } = new_loc;
    [JsonPropertyName("prev")] public CardLocations Prev { get; set; } = starting_loc;
}

public class StatUpdater(Buff buff, bool inverse) : ClientUpdater
{
    [JsonPropertyName("buff")] public Buff Buff { get; set; } = buff;
    [JsonPropertyName("inverse")] public bool Inverse { get; set; } = inverse;
}