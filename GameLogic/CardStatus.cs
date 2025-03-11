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

    public Action<CardStatus, Game>? OnPlay = data.OnPlay;
    public Action<CardStatus, Game>? OnDeath = data.OnDeath;
    public Action<CardStatus, Game>? OnAttack = data.OnAttack;
    public Action<CardStatus, Game>? OnDraw = data.OnDraw;
    
    public Action<CardStatus, Game>? on_board_effects = data.on_board_effects;
    public Action<CardStatus, Game>? in_deck_effects = data.in_deck_effects;
    public Action<CardStatus, Game>? in_void_effects = data.in_void_effects;
}