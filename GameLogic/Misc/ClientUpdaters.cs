using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[JsonDerivedType(typeof(CardLocationUpdater), "cardlocationupdater")]
[JsonDerivedType(typeof(StatUpdater), "statupdater")]
[JsonDerivedType(typeof(TurnUpdater), "turnupdator")]
[JsonDerivedType(typeof(DamageUpdater), "damageupdator")]
[JsonDerivedType(typeof(NewCardUpdater), "newcardupdator")]

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

public class StatUpdater(Buff buff, int card_id, bool inverse) : ClientUpdater
{
    [JsonPropertyName("buff")] public Buff Buff { get; set; } = buff;
    [JsonPropertyName("card_id")] public int Card_Id { get; set; } = card_id;
    [JsonPropertyName("inverse")] public bool Inverse { get; set; } = inverse;
}

public class NewCardUpdater(CardEntity card) : ClientUpdater
{
    [JsonPropertyName("card")] public CardEntity card { get; set; } = card;
}

public class DamageUpdater(int damage) : ClientUpdater
{
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class TurnUpdater(int plr_id) : ClientUpdater
{
    [JsonPropertyName("turn")] public string Turn { get; set; } = "";
    [JsonPropertyName("plr_id")] public int Plr_Id { get; set; } = plr_id;
}