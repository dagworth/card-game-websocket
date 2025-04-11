using System.Text.Json.Serialization;

public enum Passives {
    Deadly,
    Charge,
    Flying
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardTypes {
    Spell,
    FastSpell,
    Unit,
}

public enum TargetTypes {
    Spell,
    Unit,

    Player, //no targetlist

    Enemy, //Unit but includes Player
    Ally //same as Enemy
}

public enum Tribes {
    None, //should never be used on a card
    Zombie,
    Skeleton,
    Creeper
}