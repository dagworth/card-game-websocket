using System;

public class CardStatus(int card_id, CardData data) {
    public int card_id = card_id;
    public CardTypes type = data.type;
    public string name = data.name;
    public int cost = data.cost;
    public int health = data.health;
    public int max_health = data.health;
    public int attack = data.attack;
    public List<Passives> passives = [..data.passives];

    public Action<Game, CardStatus>? OnSpawn = data.OnSpawn;
    public Action<Game, CardStatus>? OnDeath = data.OnDeath;
    public Action<Game, CardStatus>? OnAttack = data.OnAttack;
    public Action<Game, CardStatus>? OnDraw = data.OnDraw;
    
    public Action<Game, CardStatus>? on_board_effects = data.on_board_effects;
    public Action<Game, CardStatus>? in_deck_effects = data.in_deck_effects;
    public Action<Game, CardStatus>? in_void_effects = data.in_void_effects;
}