public class CardData {
    public CardTypes type;
    public string name = "";
    public string description = "";
    public int cost;
    public int health;
    public int attack;
    public List<Passives> passives = [];

    public Action<CardStatus, Game>? OnPlay;
    public Action<CardStatus, Game>? OnDeath;
    public Action<CardStatus, Game>? OnAttack;
    public Action<CardStatus, Game>? OnDraw;

    public Action<CardStatus, Game>? on_board_effects;
    public Action<CardStatus, Game>? in_deck_effects;
    public Action<CardStatus, Game>? in_void_effects;
}