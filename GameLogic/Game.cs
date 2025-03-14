using Fleck;

enum GameState {
    Defending,
    Attacking,
    RegularTurn,
    PriorityTurn
}

public class Game {
    private Player plr1;
    private Player plr2;

    private int game_id;
    private int card_id_counter = 0;
    public readonly Dictionary<int,CardStatus> cards = [];

    //card_id
    public event Action<int>? OnSacrifice;

    //card_id
    public event Action<int>? OnDeath;

    //card_id
    public event Action<int>? OnAttack;

    private int plr_turn;
    private GameState game_state = GameState.RegularTurn;

    private int? plr_action_priority; //im going to ignore this as i want to only work on units for now
    private readonly List<CardStatus> on_hold_cards = []; //same with this

    //this function needs way more work cus it doesnt support like
    //fast spells and ambush
    //i also need to figure out how to implement player choice targetting ex. "give a unit +1/+1"
    public void ExecutePlayCard(PlayCard data){
        CardStatus card = GetCard(data.CardId);
        if(plr1.GetMana() < card.cost) return;

        if(plr_turn != data.PlayerId){
            if(plr_action_priority == data.PlayerId){
                GetPlayer(data.PlayerId).PlayCard(data.CardId, data.Targets);
            }
        } else {
            GetPlayer(data.PlayerId).PlayCard(data.CardId, data.Targets);
        }
    }

    public void ExecuteEndTurn(Message data){
        switch(game_state){
            case GameState.Attacking:
                if(data.PlayerId == plr_turn){
                    game_state = GameState.Defending;
                }
                break;
            case GameState.Defending:
                if(data.PlayerId != plr_turn){
                    HandleAttackPhase();
                    GetPlayer(plr_turn).Attacked();
                    game_state = GameState.RegularTurn;
                }
                break;
            case GameState.RegularTurn:
                if(data.PlayerId == plr_turn){
                    EndTurn();
                }
                break;
            case GameState.PriorityTurn:
                break;
        }
    }

    private Dictionary<int,List<int>> attacking_units = [];

    public void HandleAttackPhase(){
        foreach (KeyValuePair<int,List<int>> pair in attacking_units){
            if(pair.Value.Count == 0){
                GetCard(pair.Key).AttackPlayer(GetOtherPlayer(plr_turn));
            } else {
                //i have no idea if this works but if it does, that is very huge
                GetCard(pair.Key).AttackUnits(pair.Value.Select(GetCard).ToList());
            }
        }
    }

    public void ExecuteToggleAttack(ToggleAttack data){
        if(plr_turn != data.PlayerId) return; //if ur the attacker, can only attack on your turn
        if(GetPlayer(data.PlayerId).DidAttack()) return; //if you attacked already
        if(!(game_state is GameState.Attacking or GameState.RegularTurn)) return; //check the right gamestate

        Player plr = GetPlayer(plr_turn);
        if(plr.FindIndex(plr.Board, data.UnitAttacking) == -1) return; //check the unit is on board

        game_state = GameState.Attacking;
        attacking_units[data.UnitAttacking] = [];
    }

    public void ExecuteReverseToggleAttack(ReverseToggleAttack data){
        if(plr_turn != data.PlayerId) return; //if ur the attacker
        if(game_state is not GameState.Attacking) return; //check the right gamestate

        attacking_units.Remove(data.UnitAttacking);
        if(attacking_units.Count == 0){
            game_state = GameState.RegularTurn;
        }
    }

    public void ExecuteToggleDefend(ToggleDefend data){
        if(plr_turn == data.PlayerId) return; //if ur the defender
        if(game_state is not GameState.Defending) return; //check the right gamestate

        Player plr = GetOtherPlayer(plr_turn);
        if(plr.FindIndex(plr.Board, data.UnitDefending) == -1) return; //check the unit is on board
        if(!attacking_units.ContainsKey(data.UnitAttacking)) return; //the specified unit getting defended is not attacking

        //check if unit is already defending anything
        foreach (KeyValuePair<int,List<int>> pair in attacking_units){
            if(pair.Value.Contains(data.UnitDefending)) return;
        }
        
        attacking_units[data.UnitAttacking].Add(data.UnitDefending);
    }

    public void ExecuteReverseToggleDefend(ReverseToggleDefend data){
        if(plr_turn == data.PlayerId) return; //if ur the defender
        if(game_state is not GameState.Defending) return; //check the right gamestate

        if(!attacking_units[data.UnitAttacking].Contains(data.UnitDefending)) return; //the unit is not defending the attacking unit
        
        attacking_units[data.UnitAttacking].Remove(data.UnitDefending);
    }

    public void EndTurn(){
        if(plr1.GetId() == plr_turn){
            plr_turn = plr2.GetId();
        } else {
            plr_turn = plr1.GetId();
        }
    }

    public CardStatus AddCard(string card_name){
        int id = card_id_counter++;
        CardStatus new_card = new CardStatus(id,CardStatStorage.GetCardData(card_name));
        cards[id] = new_card;
        return new_card;
    }

    public CardStatus GetCard(int id){
        return cards[id];
    }

    public Game(int game_id, int plr1_id, int plr2_id, List<string> deck1, List<string> deck2){
        this.game_id = game_id;

        plr1 = new Player(this, plr1_id);
        plr2 = new Player(this, plr2_id);

        GameManager.AddPlayer(plr1_id, plr1);
        GameManager.AddPlayer(plr2_id, plr2);

        plr_turn = plr1_id;

        //convert into CardStatus
        for(int i = 0; i < deck1.Count; i++){
            plr1.Deck.Add(AddCard(deck1[i]));
        }
        for(int i = 0; i < deck2.Count; i++){
            plr2.Deck.Add(AddCard(deck2[i]));
        }

        for(int i = 0; i < 3; i++){
            plr1.DrawCard();
            plr2.DrawCard();
        }

        Console.WriteLine($"game was made! {plr1.GetId()} {plr2.GetId()} {plr1.Deck.Count} {plr2.Deck.Count}");
    }

    public void SacrificeCard(int card_id){
        Player plr = plr1.FindIndex(plr1.Board, card_id) != -1 ? plr1 : plr2;
        int board_id = plr.FindIndex(plr.Board, card_id);

        CardStatus card = plr.Board[board_id];
        plr.Board.RemoveAt(board_id);
        plr.Void.Add(card);

        OnSacrifice?.Invoke(card_id);
        OnDeath?.Invoke(card_id);
    }

    public void ReturnFromVoid(int card_id){
        Player plr = plr1.FindIndex(plr1.Void, card_id) != -1 ? plr1 : plr2;
        int board_id = plr.FindIndex(plr.Void, card_id);

        CardStatus card = plr.Void[board_id];
        plr.Void.RemoveAt(board_id);

        plr.SpawnCard(card);
    }

    public List<CardStatus> GetAllCardsOnBoard(){
        List<CardStatus> card_list = [];
        card_list.AddRange(plr1.Board);
        card_list.AddRange(plr2.Board);
        return card_list;
    }

    public Player GetPlayer(int plr_id){
        if(plr1.GetId() == plr_id) return plr1;
        return plr2;
    }

    public Player GetOtherPlayer(int plr_id){
        if(plr1.GetId() == plr_id) return plr2;
        return plr1;
    }

    public void PrintState(){
        Console.WriteLine($"\nplr {plr_turn} turn\n");
        Console.WriteLine($"gamestate {game_state}");
        Console.WriteLine($"plr1 health {plr1.GetHealth()}");
        Console.WriteLine($"plr2 health {plr2.GetHealth()}");

        Console.WriteLine($"plr {plr1.GetId()} hand");
        foreach(CardStatus card in plr1.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr1.GetId()} board");
        foreach(CardStatus card in plr1.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr2.GetId()} hand");
        foreach(CardStatus card in plr2.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {plr2.GetId()} board");
        foreach(CardStatus card in plr2.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }
    }
}