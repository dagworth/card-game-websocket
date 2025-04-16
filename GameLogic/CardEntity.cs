using System;

public class CardEntity(GameEntity game, int plr_id, int card_id, string name, CardData data) {
    private readonly GameEntity game = game;

    public readonly int Id = card_id;
    public readonly int Plr_Id = plr_id; //temp readonly

    private readonly List<Buff> buffs = [];

    public CardTypes Type { get; private set; } = data.type;
    public CardLocations Location { get; private set; } = CardLocations.Deck;
    public string Name { get; private set; } = name;
    public string Desc { get; private set; } = data.description;

    public readonly List<Tribes> Tribes = [..data.tribes];

    public CardStats Stats = new CardStats();
    private CardStats perm_stats = new CardStats(data);

    public Action<GameEntity, PlayerEntity, CardEntity, List<int>>? OnPlay = data.OnPlay; //targetting part should be ignored for now
    public Action<GameEntity, PlayerEntity, CardEntity>? OnSpawn = data.OnSpawn;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDeath = data.OnDeath;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnAttack = data.OnAttack;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDraw = data.OnDraw;
    
    public Action<GameEntity, PlayerEntity, CardEntity>? custom_effects = data.custom_effects;

    public void UpdateStats(){
        int cost = perm_stats.Cost;
        int hp = perm_stats.Health;
        int atk = perm_stats.Attack;
        List<Passives> p = [..perm_stats.passives];

        foreach(Buff buff in buffs){
            atk += buff.Attack;
            hp += buff.Health;
            foreach(Passives passive in buff.passives){
                if(!p.Contains(passive)){
                    p.Add(passive);
                }
            }
        }

        Stats.Cost = cost;
        Stats.Health = hp;
        Stats.Attack = atk;
        Stats.passives = p;

        if(Type == CardTypes.Unit) CheckIfDead();
    }

    public Buff AddTempBuff(Buff buff){
        buff.card = this;
        buffs.Add(buff);
        UpdateStats();
        return buff;
    }

    public void RemoveTempBuff(Buff buff){
        buffs.Remove(buff);
        UpdateStats();
    }

    public void AddPermBuff(Buff buff){
        perm_stats.Cost += buff.Cost;
        perm_stats.Health += buff.Health;
        perm_stats.Attack += buff.Attack;
        foreach(Passives p in buff.passives){
            if(!perm_stats.passives.Contains(p)){
                perm_stats.passives.Add(p);
            }
        }
        UpdateStats();
    }

    public void TakeDamage(int damage){
        Stats.Damaged += damage;
    }

    public void CheckIfDead(){
        if(Stats.Damaged >= Stats.Health) game.events.KillCard(Id);
    }

    public void SetLocation(CardLocations loc){
        Location = loc;
    }
    
    //returns how much attack is left
    public int AttackCard(CardEntity victim, int atk){
        TakeDamage(victim.Stats.Attack);

        if(victim.Stats.Health - atk >= 0){
            victim.TakeDamage(atk);
            atk = 0;
        } else {
            victim.TakeDamage(victim.Stats.Health);
            atk -= victim.Stats.Health;
        }
        
        return atk;
    }
}