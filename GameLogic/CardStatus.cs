using System;

public class CardStatus(int card_id, Game game, int plr_id, CardData data) {
    public int card_id = card_id;
    public Game game = game;
    public int plr_id = plr_id;
    public CardTypes type = data.type;
    public Tribes tribe = data.tribe;
    public string name = data.name;
    public int cost = data.cost;
    public int health = data.health;
    public int max_health = data.health;
    public int attack = data.attack;
    public List<Passives> passives = [..data.passives];

    public List<EventHandler> eventHandlers = [];

    public Action<Game, Player, CardStatus>? OnSpawn = data.OnSpawn;
    public Action<Game, Player, CardStatus>? OnDeath = data.OnDeath;
    public Action<Game, Player, CardStatus>? OnAttack = data.OnAttack;
    public Action<Game, Player, CardStatus>? OnDraw = data.OnDraw;
    
    public Action<Game, Player, CardStatus>? custom_effects = data.custom_effects;

    public void Attack(List<CardStatus> victims){
        int atk = attack;

        if(victims.Count == 0){
            game.GetOtherPlayer(plr_id).ChangeHealth(-atk);
            return;
        }

        foreach(CardStatus victim in victims){
            if(victim.health - atk > 0){
                victim.health -= atk;
                atk = 0;
            } else {
                victim.health = 0;
                atk -= victim.health;
            }
        }
    }

    public void ChangeStats(int atk, int hp){
        health+=hp;
        max_health+=hp;
        attack+=atk;
    }
}