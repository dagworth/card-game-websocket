public class CardData {
    public CardTypes type;
    public string name = "";
    public string description = "";
    public int cost;
    public int health;
    public int attack;
    public List<Passives> passives = [];

    public Action<Game, CardStatus>? OnSpawn;
    public Action<Game, CardStatus>? OnDeath;
    public Action<Game, CardStatus>? OnAttack;
    public Action<Game, CardStatus>? OnDraw;

    public Action<Game, CardStatus>? on_board_effects;
    public Action<Game, CardStatus>? in_deck_effects;
    public Action<Game, CardStatus>? in_void_effects;
}