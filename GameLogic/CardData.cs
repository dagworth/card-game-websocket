public class CardData {
    public CardTypes type;
    public Tribes tribe;
    public string name = "";
    public string description = "";
    public int cost;
    public int health;
    public int attack;
    public List<Passives> passives = [];

    public Action<Game, Player, CardStatus>? OnSpawn;
    public Action<Game, Player, CardStatus>? OnDeath;
    public Action<Game, Player, CardStatus>? OnAttack;
    public Action<Game, Player, CardStatus>? OnDraw;

    public Action<Game, Player, CardStatus>? custom_effects;
}