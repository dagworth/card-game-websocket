namespace shared;

using System.Text.Json.Serialization;
using shared.DTOs;

[JsonDerivedType(typeof(CardLocationUpdater), "locationchange")]
[JsonDerivedType(typeof(StatUpdater), "statchange")]
[JsonDerivedType(typeof(TurnUpdater), "turnchange")]
[JsonDerivedType(typeof(DamageUpdater), "damagechange")]
[JsonDerivedType(typeof(NewCardUpdater), "newcard")]

public class ClientUpdater { }

public class CardLocationUpdater(int cardid, CardLocations now, CardLocations prev) : ClientUpdater {
    [JsonPropertyName("card_id")] public int CardId { get; set; } = cardid;
    [JsonPropertyName("now")] public CardLocations Now { get; set; } = now;
    [JsonPropertyName("prev")] public CardLocations Prev { get; set; } = prev;
}

public class StatUpdater(int cardid, BuffDTO buff, bool inverse) : ClientUpdater {
    [JsonPropertyName("buff")] public BuffDTO Buff { get; set; } = buff;
    [JsonPropertyName("card_id")] public int CardId { get; set; } = cardid;
    [JsonPropertyName("inverse")] public bool Inverse { get; set; } = inverse;
}

public class NewCardUpdater(CardEntityDTO card) : ClientUpdater {
    [JsonPropertyName("card")] public CardEntityDTO card { get; set; } = card;
}

public class DamageUpdater(int damage) : ClientUpdater {
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class TurnUpdater(int plrid) : ClientUpdater {
    [JsonPropertyName("turn")] public string Turn { get; set; } = "";
    [JsonPropertyName("plr_id")] public int PlrId { get; set; } = plrid;
}

// public class EnemyCardUpdater(int card_count) : ClientUpdater {
//     [JsonPropertyName("card_count")] public int Card_Count { get; set; } = card_count;
// }