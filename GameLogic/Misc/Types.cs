using System.Text.Json.Serialization;

public enum Passives {
    Deadly,
    Charge,
    Flying
}

public enum CardLocations {
    Hand,
    Deck,
    Void,
    Board
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardTypes {
    Spell,
    FastSpell,
    Unit,
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
    None, //should never be used on a card
    Zombie,
    Skeleton,
    Creeper
}