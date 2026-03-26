namespace shared;

using System.Text.Json.Serialization;
using shared.DTOs;

[JsonDerivedType(typeof(CardLocationUpdater), "locationchange")]
[JsonDerivedType(typeof(ToggleAttackUpdater), "toggleattack")]
[JsonDerivedType(typeof(StatUpdater), "statchange")]
[JsonDerivedType(typeof(TurnUpdater), "turnchange")]
[JsonDerivedType(typeof(AttackActionUpdater), "attackaction")]
[JsonDerivedType(typeof(NewCardUpdater), "newcard")]
[JsonDerivedType(typeof(CardDamageUpdater), "carddamaged")]
[JsonDerivedType(typeof(PlrDamageUpdater), "plrdamaged")]
[JsonDerivedType(typeof(ManaUpdater), "manachange")]

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

public class ToggleAttackUpdater(int cardid, bool status) : ClientUpdater {
    [JsonPropertyName("card_id")] public int CardId { get; set; } = cardid;
    [JsonPropertyName("status")] public bool Status { get; set; } = status;
}

public class AttackActionUpdater(int attacker, int defender, int damage) : ClientUpdater {
    [JsonPropertyName("attacker")] public int Attacker { get; set; } = attacker;
    [JsonPropertyName("defender")] public int Defender { get; set; } = defender;
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class CardDamageUpdater(int cardid, int damage) : ClientUpdater {
    [JsonPropertyName("card_id")] public int CardId { get; set; } = cardid;
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class PlrDamageUpdater(int plrid, int damage) : ClientUpdater {
    [JsonPropertyName("plr_id")] public int PlrId { get; set; } = plrid;
    [JsonPropertyName("damage")] public int Damage { get; set; } = damage;
}

public class ManaUpdater(int plrid, int max, int value) : ClientUpdater {
    [JsonPropertyName("plr_id")] public int PlrId { get; set; } = plrid;
    [JsonPropertyName("max")] public int Max { get; set; } = max;
    [JsonPropertyName("value")] public int Value { get; set; } = value;
}

public class TurnUpdater(int plrid) : ClientUpdater {
    [JsonPropertyName("plr_id")] public int PlrId { get; set; } = plrid;
}

// public class EnemyCardUpdater(int card_count) : ClientUpdater {
//     [JsonPropertyName("card_count")] public int Card_Count { get; set; } = card_count;
// }





/*

the names of each of these serverevents and stuff are only for the client to do animations and know what is happening, it does not effect anything else

gameupdate, updatetype informcards, events:
(
    [drawcards events:
        newcard 1
        newcard 2
        newcard 3
    ]
)

(serverevent) gameupdate, updatetype attackphase, events:
(
    (client updater) [unitattack events:
        (update event) plrdamaged, damage 10
        unitdamaged 1 14
        location change 1 4
    ]

    [unitattack  events:
        plrdamaged, damage 10
        unitdamaged 1 14
        location change 1 4
    ]
)

do the serverevent update right agameupdatefter the invoke of the cardeffect signal, like onsacrifice, so that it will bunch up all the new changes
gameupdate, updatetype cardeffects, events:
(
    [cardeffect events:
        buff, 1, 1 1
        buff, 1, 1 1
    ]

    [cardeffect events:
        plrdamaged, damage 10
    ]
)

3 tiers
gameupdate
clientupdater
updateevent

*/