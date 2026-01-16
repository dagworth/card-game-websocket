namespace shared;

using System.Text.Json.Serialization;
using shared.DTOs;

[JsonDerivedType(typeof(CardLocationUpdater), "locationchange")]
[JsonDerivedType(typeof(StatUpdater), "statchange")]
[JsonDerivedType(typeof(TurnUpdater), "turnchange")]
[JsonDerivedType(typeof(DamageUpdater), "damagechange")]
[JsonDerivedType(typeof(NewCardUpdater), "newcard")]

public class ClientUpdater { }

public class CardLocationUpdater(int card_id, CardLocations new_loc, CardLocations starting_loc) : ClientUpdater {
    [JsonPropertyName("card_id")] public int Card_Id { get; set; } = card_id;
    [JsonPropertyName("now")] public CardLocations Now { get; set; } = new_loc;
    [JsonPropertyName("prev")] public CardLocations Prev { get; set; } = starting_loc;
}

public class StatUpdater(int card_id, BuffDTO buff, bool inverse) : ClientUpdater {
    [JsonPropertyName("buff")] public BuffDTO Buff { get; set; } = buff;
    [JsonPropertyName("card_id")] public int Card_Id { get; set; } = card_id;
    [JsonPropertyName("inverse")] public bool Inverse { get; set; } = inverse;
}

public class NewCardUpdater(CardEntityDTO card) : ClientUpdater {
    [JsonPropertyName("card")] public CardEntityDTO card { get; set; } = card;
}

public class DamageUpdater(int damage) : ClientUpdater {
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class TurnUpdater(int plr_id) : ClientUpdater {
    [JsonPropertyName("turn")] public string Turn { get; set; } = "";
    [JsonPropertyName("plr_id")] public int Plr_Id { get; set; } = plr_id;
}