namespace server.GameLogic.Entities;

using server.GameLogic.Interfaces;
using shared.DTOs;

using System;

public class CardEntity : IDamageable {
    private readonly GameEntity Game;

    public int Id;
    public readonly int Plr_Id; //temp readonly

    private readonly List<Buff> buffs = [];

    public CardTypes Type;
    public CardLocations Location;
    public string Name;

    public readonly List<Tribes> Tribes;

    public CardStats Stats; //stats after modifiers
    private CardStats perm_stats; //the starter stats

    public Action<GameEntity, PlayerEntity, CardEntity>? OnPlay; 
    public Action<GameEntity, PlayerEntity, CardEntity>? OnSpawn;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDeath;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnAttack;
    public Action<GameEntity, PlayerEntity, CardEntity>? OnDraw;

    public Action<GameEntity, PlayerEntity, CardEntity>? custom_effects;

    public CardEntity(GameEntity game, int plr_id, int card_id, string name, CardDataDTO data) {
        Game = game;
        Id = card_id;
        Plr_Id = plr_id;
        Type = data.Type;
        Location = CardLocations.Deck;
        Name = name;
        Tribes = [.. data.Tribes];
        Stats = new();
        perm_stats = new(data);
        BindLogic(DataLogicLoader.GetLogic(name));
    }

    public void BindLogic(CardEffect e) {
        if (DataLogicLoader.HasEffect(Name, nameof(CardEffect.OnSpawn))) OnSpawn = e.OnSpawn;
        if (DataLogicLoader.HasEffect(Name, nameof(CardEffect.OnDeath))) OnDeath = e.OnDeath;
        if (DataLogicLoader.HasEffect(Name, nameof(CardEffect.OnAttack))) OnAttack = e.OnAttack;
        if (DataLogicLoader.HasEffect(Name, nameof(CardEffect.OnPlay))) OnPlay = e.OnPlay;
    }

    public void UpdateStats() {
        int cost = perm_stats.Cost;
        int hp = perm_stats.Health;
        int atk = perm_stats.Attack;
        List<Passives> p = [.. perm_stats.passives];

        foreach (Buff buff in buffs) {
            atk += buff.Attack;
            hp += buff.Health;
            cost += buff.Cost;
            foreach (Passives passive in buff.passives) {
                if (!p.Contains(passive)) {
                    p.Add(passive);
                }
            }
        }

        Stats.Cost = cost;
        Stats.Health = hp;
        Stats.Attack = atk;
        Stats.passives = p;

        if (Type == CardTypes.Unit)
            CheckIfDead();
    }

    public Buff AddTempBuff(Buff buff) {
        buff.card = this;
        Game.updater.ChangeStats(buff, Id, 0);
        buffs.Add(buff);
        UpdateStats();
        return buff;
    }

    public void RemoveTempBuff(Buff buff) {
        Game.updater.ChangeStats(buff, Id, 0);
        buffs.Remove(buff);
        UpdateStats();
    }

    public void AddPermBuff(Buff buff) {
        buff.card = this;
        Game.updater.ChangeStats(buff, 0);

        //manually change stuff for buff, update if needed
        perm_stats.Cost += buff.Cost;
        perm_stats.Health += buff.Health;
        perm_stats.Attack += buff.Attack;
        foreach (Passives p in buff.passives) {
            if (!perm_stats.passives.Contains(p)) {
                perm_stats.passives.Add(p);
            }
        }

        UpdateStats();
    }

    public void TakeDamage(int damage) {
        Game.updater.TookDamage(damage, 0);
        //Console.WriteLine($"{Name} took {damage} damage");
        Stats.Damaged += damage;
        UpdateStats();
    }

    public void CheckIfDead() {
        if (Stats.Damaged >= Stats.Health)
            Game.events.KillCard(Id);
    }

    public void SetLocation(CardLocations loc) {
        Location = loc;
    }

    //returns how much attack is left for future overwhelm
    public int AttackCard(CardEntity victim, int atk) {
        Game.events.InvokeOnAttack(Id); //idk if this will work but i dont think it matters for now
        TakeDamage(victim.Stats.Attack);

        if (victim.Stats.Health - atk >= 0) {
            victim.TakeDamage(atk);
            atk = 0;
        } else {
            victim.TakeDamage(victim.Stats.Health);
            atk -= victim.Stats.Health;
        }

        return atk;
    }
}
