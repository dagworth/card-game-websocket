using System.Text.Json.Serialization;

public enum Passives {
    Deadly,
    Charge
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardTypes {
    Spell,
    FastSpell,
    Unit
}

public enum Tribes {
    Zombie,
    Skeleton,
    Creeper
}