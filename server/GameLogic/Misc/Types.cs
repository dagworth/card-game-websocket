using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Passives {
    Charge = 0,
    Flying = 1,
    Deadly = 2
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardLocations {
    Hand = 0,
    Deck = 1,
    Void = 2,
    Board = 3
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardTypes {
    Unit = 0,
    Spell = 1,
    FastSpell = 2
}

public enum Delays {
    EndTurn,
    EndOppTurn,
    StartTurn,
    StartOppTurn
}

public enum TargetTypes {
    YourUnitsInHand,
    EnemyUnitsInHand,

    YourSpellsInHand,
    EnemySpellsInHand,

    YourUnitsInVoid,
    EnemyUnitsInVoid,

    YourSpellsInVoid,
    EnemySpellsInVoid,

    YourUnits,
    EnemyUnits,

    YourPlayer,
    EnemyPlayer,

    Enemies,
    Allies,
    Anything,

    PlayedFastSpell,
    PlayedSpell
}

public enum Tribes {
    None = 0, //should never be used on a card
    Zombie = 1,
    Skeleton = 2,
    Creeper = 3
}