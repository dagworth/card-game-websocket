public class CardData {
    public CardTypes type;
    public string description = "";
    public int cost;
    public int health;
    public int attack;

    public List<Tribes> tribes = [];
    public List<Passives> passives = [];

    public Action<GameEntity, PlayerEntity, CardEntity, List<int>>? OnPlay;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnSpawn;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDeath;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnAttack;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDraw;

    public Action<GameEntity, PlayerEntity, CardEntity>? custom_effects;

    public ChooseTargetsParams? OnPlayTargettingParams; //to do
}