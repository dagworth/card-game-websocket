using System;

public struct Buff(int atk, int hp, List<Passives> p) {
    public readonly int atk = atk;
    public readonly int hp = hp;
    public readonly List<Passives> p = p;
}

public class CardStatus(int card_id, Game game, int plr_id, CardData data) {
    public readonly int Id = card_id;
    public readonly Game Game = game;

    public int Plr_Id { get; private set; } = plr_id;
    public CardTypes Type { get; private set; } = data.type;
    public CardLocations Location { get; private set; } = CardLocations.Deck;
    public string Name { get; private set; } = data.name;
    public int Cost { get; private set; } = data.cost;
    public int Health { get; private set; } = data.health;
    public int Max_Health { get; private set; } = data.health;
    public int Attack { get; private set; } = data.attack;

    private readonly List<Tribes> tribes = [..data.tribes];
    private readonly List<Passives> passives = [..data.passives];

    public Action<Game, Player, CardStatus, List<int>>? OnPlay = data.OnPlay;
    public Action<Game, Player, CardStatus>? OnSpawn = data.OnSpawn;
    public Action<Game, Player, CardStatus>? OnDeath = data.OnDeath;
    public Action<Game, Player, CardStatus>? OnAttack = data.OnAttack;
    public Action<Game, Player, CardStatus>? OnDraw = data.OnDraw;
    
    public Action<Game, Player, CardStatus>? custom_effects = data.custom_effects;

    public bool HasTribe(Tribes tribe){
        return tribes.Contains(tribe);
    }

    public bool HasPassive(Passives passive){
        return passives.Contains(passive);
    }

    public void GivePassive(Passives passive){
        passives.Add(passive);
    }

    public void GiveTribe(Tribes tribe){
        tribes.Add(tribe);
    }

    public void TakeDamage(int damage){
        Health -= damage;
    }

    public void SetLocation(CardLocations loc){
        Location = loc;
    }

    public void AttackEnemies(List<CardStatus> victims){
        int atk = Attack;

        if(victims.Count == 0){
            Game.plrs.GetOtherPlayer(Plr_Id).ChangeHealth(-atk);
            return;
        }

        foreach(CardStatus victim in victims){
            atk = AttackCard(victim, atk);
        }
    }
    
    //returns how much attack is left
    public int AttackCard(CardStatus victim, int atk){
        TakeDamage(victim.Attack);

        if(victim.Health - atk >= 0){
            victim.TakeDamage(atk);
            atk = 0;
        } else {
            victim.TakeDamage(victim.Health);
            atk -= victim.Health;
        }
        
        return atk;
    }

    public Buff MakeBuff(int atk, int hp){
        Health += hp;
        Max_Health += hp;
        Attack += atk;

        return new Buff(atk,hp,[]);
    }

    public Buff MakeBuff(int atk, int hp, List<Passives> p){
        Health += hp;
        Max_Health += hp;
        Attack += atk;

        return new Buff();
    }

    public void RemoveBuff(Buff buff){

    }
}