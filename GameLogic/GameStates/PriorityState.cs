public readonly struct CardEffect(int plr_id, CardStatus? trigger, Action<List<int>> effect, List<int> input){
    public readonly int plr_id = plr_id;
    public readonly CardStatus? Trigger = trigger;
    public readonly Action<List<int>> Effect = effect;
    public readonly List<int> Input = input;
}

public class PriorityState(Game game, int plr_id, IGameState old_state) : IGameState {
    private readonly Game game = game;
    private int plr_priority = plr_id;
    private readonly IGameState old_state = old_state;
    private readonly List<CardEffect> on_hold_card_effects = [];

    public void StateStarted(){
        Check();
    }

    public void Check(){
        bool can = false;
        Player plr = game.GetPlayer(plr_priority);
        for(int i = 0; i < plr.Hand.Count; i++){
            if(plr.Hand[i].cost > plr.Mana) continue;
            if(plr.Hand[i].type > CardTypes.FastSpell) continue;
            
            can = true;
            break;
        }

        if(!can) EndTurn();
    }

    public void AddEffect(CardEffect effect){
        on_hold_card_effects.Add(effect);
        plr_priority = game.GetOtherPlayer(plr_priority).Id;
        Check();
    }

    public bool CanPlayCard(CardStatus card){
        if(plr_priority != card.plr_id) return false;
        if(card.type != CardTypes.FastSpell) return false;
        if(card.cost > game.GetPlayer(plr_priority).Mana) return false;

        return true;
    }

    public void EndTurn(){
        if(on_hold_card_effects.Count == 0){
            game.SetGameState(old_state);
            return;
        }

        if(on_hold_card_effects.Last().plr_id != plr_priority){
            DoCardEffects();
            Console.WriteLine("back to defend");
            game.SetGameState(old_state);
        }  else {
            plr_priority = game.GetOtherPlayer(plr_priority).Id;
        }
    }

    public void DoCardEffects(){
        while(on_hold_card_effects.Count > 0){
            int i = on_hold_card_effects.Count-1;
            CardEffect a = on_hold_card_effects[i];
            a.Effect.Invoke(a.Input);
            on_hold_card_effects.RemoveAt(on_hold_card_effects.Count-1);
        }
    }
}