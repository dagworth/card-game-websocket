using System.Buffers;
using Fleck;

public readonly struct CardEffect(int plr_id, CardStatus? trigger, Action<List<int>> effect, List<int> input){
    public readonly int plr_id = plr_id;
    public readonly CardStatus? Trigger = trigger;
    public readonly Action<List<int>> Effect = effect;
    public readonly List<int> Input = input;
}

public class Game {
    private int card_id_counter = 0;
    public int Id { get; private set; }

    public Player Plr1 { get; private set; }
    public Player Plr2 { get; private set; }

    public readonly Dictionary<int,CardStatus> Cards = [];

    public event Action<int>? OnSacrifice;
    public event Action<int>? OnDeath;
    public event Action<int>? OnAttack;
    public event Action<int>? OnSpawn;

    public int Plr_Turn { get; private set; }
    public int Plr_Turn_Priority { get; private set; }
    public int Plr_Choosing { get; private set; }
    
    public GameState Game_State { get; set; } = GameState.RegularTurn;

    private List<int> Plr_Choice_List;
    private Action<List<int>> Plr_Choice_Effect;

    private readonly List<CardEffect> on_hold_card_effects = [];

    public readonly Dictionary<int,List<int>> Attacking_Units = [];

    public void DoCardEffects(){
        while(on_hold_card_effects.Count > 0){
            int i = on_hold_card_effects.Count-1;
            CardEffect a = on_hold_card_effects[i];
            a.Effect.Invoke(a.Input);
        }
    }

    public void MakeCounterableEffect(int plr_id, Action<List<int>> func, List<int> input){
        on_hold_card_effects.Add(new CardEffect(
            plr_id,
            null, //change the ownership of the card effect later, this doesnt matter usually
            func,
            input
        ));
        Plr_Turn_Priority = GetOtherPlayer(plr_id).Id;
        Game_State = GameState.PriorityTurn;
    }

    public void QueryPlrTargets(int plr_id, Action<List<int>> func, ChooseTargetsParams info){
        info.Filter();
        Game_State = GameState.PlrChoosing;
        Plr_Choosing = plr_id;
        Plr_Choice_List = info.TargetList;
        Plr_Choice_Effect = func;
        MessageHandler.AskForTargets(ServerHandler.GetWSConnection(plr_id), info.TargetList);
    }

    public void GotPlrTargets(int plr_id, TargetsChoice info){
        if(Plr_Choosing == plr_id){
            bool valid = true;
            foreach (int c in info.Targets){
                if(!Plr_Choice_List.Contains(c)){
                    valid = false;
                    break;
                }
            }

            if(!valid) return;

            Game_State = GameState.RegularTurn;

            Plr_Choice_Effect.Invoke(info.Targets);
            Plr_Choosing = -1;
        }
    }


    public void HandleAttackPhase(){
        MakeCounterableEffect(
            Plr_Turn,
            (list) => {
                foreach(KeyValuePair<int,List<int>> pair in Attacking_Units){
                    GetCard(pair.Key).AttackEnemies([..pair.Value.Select(GetCard)]);
                }
            },
            []
        );
    }

    public void ChangeTurn(){
        Plr_Turn = GetOtherPlayer(Plr_Turn).Id;
    }

    public CardStatus CreateCard(string card_name, int plr_id){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id, this, plr_id, CardStatStorage.GetCardData(card_name));
        Cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int card_id){
        return Cards[card_id];
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        Id = game_id;
        Plr_Choice_Effect = (list) => { Console.WriteLine("htis should never happen"); };

        Plr1 = new Player(this, plr1_id);
        Plr2 = new Player(this, plr2_id);

        GameManager.AddPlayer(plr1_id, Plr1);
        GameManager.AddPlayer(plr2_id, Plr2);

        Plr_Turn = plr1_id;

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            Plr1.Deck.Add(CreateCard(deck1[i], plr1_id));
        }

        for(int i = 0; i < deck2.Count; i++){
            Plr2.Deck.Add(CreateCard(deck2[i], plr2_id));
        }

        for(int i = 0; i < 3; i++){
            Plr1.DrawCard();
            Plr2.DrawCard();
        }
    }

    public void SpawnCard(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        plr.Board.Add(card);
        card.OnSpawn?.Invoke(this, plr, card); //The spawned card's OnSpawn should happen before other cards
        OnSpawn?.Invoke(card_id);
    }

    public void SacrificeCard(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        int board_id = plr.FindIndex(plr.Board, card_id);
        plr.Board.RemoveAt(board_id);

        plr.Void.Add(card);

        OnSacrifice?.Invoke(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void ReturnFromVoid(int card_id){
        CardStatus card = GetCard(card_id);
        Player plr = GetPlayer(card.plr_id);

        int void_id = plr.FindIndex(plr.Void, card_id);
        plr.Void.RemoveAt(void_id);

        SpawnCard(card_id);
    }

    public List<CardStatus> GetAllCardsOnBoard(){
        List<CardStatus> card_list = [];
        card_list.AddRange(Plr1.Board);
        card_list.AddRange(Plr2.Board);
        return card_list;
    }

    public Player GetPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr1;
        return Plr2;
    }

    public Player GetOtherPlayer(int plr_id){
        if(Plr1.Id == plr_id) return Plr2;
        return Plr1;
    }

    public Player GetOtherPlayer(Player plr){
        if(Plr1 == plr) return Plr2;
        return Plr1;
    }

    //this function needs way more work cus it doesnt support like
    //fast spells and ambush
    //i also need to figure out how to implement player choice targetting ex. "give a unit +1/+1"
    public void ExecutePlayCard(PlayCard data){
        CardStatus card = GetCard(data.CardId);
        Player plr = GetPlayer(data.PlayerId);
        if(plr.Mana < card.cost) return;

        if(Plr_Turn != data.PlayerId){
            if(Plr_Turn_Priority == data.PlayerId){
                plr.PlayCard(data.CardId);
            }
        } else {
            plr.PlayCard(data.CardId);
        }
    }

    public void ExecuteEndTurn(Message data){
        switch(Game_State){
            case GameState.Attacking:
                if(data.PlayerId == Plr_Turn){
                    Game_State = GameState.Defending;
                }
                break;
            case GameState.Defending:
                if(data.PlayerId != Plr_Turn){
                    HandleAttackPhase();
                    GetPlayer(Plr_Turn).ExecutedAttack();
                    Game_State = GameState.RegularTurn;
                }
                break;
            case GameState.RegularTurn:
                if(data.PlayerId == Plr_Turn){
                    ChangeTurn();
                }
                break;
        }
    }

    public void ExecuteToggleAttack(ToggleAttack data){
        if(Plr_Turn != data.PlayerId) return; //if ur the attacker, can only attack on your turn
        if(GetPlayer(data.PlayerId).Attacked) return; //if you attacked already
        if(!(Game_State is GameState.Attacking or GameState.RegularTurn)) return; //check the right gamestate

        Player plr = GetPlayer(Plr_Turn);
        if(plr.FindIndex(plr.Board, data.UnitAttacking) == -1) return; //check the unit is on board

        Game_State = GameState.Attacking;
        Attacking_Units[data.UnitAttacking] = [];
    }

    public void ExecuteReverseToggleAttack(ToggleAttack data){
        if(Plr_Turn != data.PlayerId) return; //if ur the attacker
        if(Game_State is not GameState.Attacking) return; //check the right gamestate

        Attacking_Units.Remove(data.UnitAttacking);
        if(Attacking_Units.Count == 0){
            Game_State = GameState.RegularTurn;
        }
    }

    public void ExecuteToggleDefend(ToggleDefend data){
        if(Plr_Turn == data.PlayerId) return; //if ur the defender
        if(Game_State is not GameState.Defending) return; //check the right gamestate

        Player plr = GetOtherPlayer(Plr_Turn);
        if(plr.FindIndex(plr.Board, data.UnitDefending) == -1) return; //check the unit is on board
        if(!Attacking_Units.ContainsKey(data.UnitAttacking)) return; //the specified unit getting defended is not attacking

        //check if unit is already defending anything
        foreach (KeyValuePair<int,List<int>> pair in Attacking_Units){
            if(pair.Value.Contains(data.UnitDefending)) return;
        }
        
        Attacking_Units[data.UnitAttacking].Add(data.UnitDefending);
    }

    public void ExecuteReverseToggleDefend(ToggleDefend data){
        if(Plr_Turn == data.PlayerId) return; //if ur the defender
        if(Game_State is not GameState.Defending) return; //check the right gamestate

        if(!Attacking_Units[data.UnitAttacking].Contains(data.UnitDefending)) return; //the unit is not defending the attacking unit
        
        Attacking_Units[data.UnitAttacking].Remove(data.UnitDefending);
    }

    public void ExecuteTargetsChoice(TargetsChoice data){
        if(Plr_Choosing != data.PlayerId) return; //if ur the chooser
        if(Game_State is not GameState.PlrChoosing) return; //check the right gamestate

        
    }
}