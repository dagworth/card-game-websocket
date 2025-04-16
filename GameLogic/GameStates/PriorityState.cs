public readonly struct CardEffect(int plr_id, CardEntity? trigger, Action effect){
    public readonly int plr_id = plr_id;
    public readonly CardEntity? Trigger = trigger;
    public readonly Action Effect = effect;
}

public class PriorityState : IGameState {
    private readonly GameEntity game;
    public int plr_priority; //public to debug
    private readonly IGameState next_state;
    private readonly List<CardEffect> on_hold_card_effects = [];

    public PriorityState(GameEntity game, int plr_priority, IGameState next_state){
        this.game = game;
        this.plr_priority = plr_priority;
        this.next_state = next_state;
    }

    public void StartState(){
        CheckLegalPlays();
    }

    public void EndTurn(){
        //if opponent did not respond to our effect (this is not optimal but good for now)
        if(on_hold_card_effects.Count == 0 || on_hold_card_effects.Last().plr_id != plr_priority){
            game.SetGameState(next_state); //this is so that if a choosing comes up in the card effects, this wont effect it
            DoCardEffects();
        }  else {
            plr_priority = game.plrs.GetOtherPlayer(plr_priority).Id;
        }
    }

    public bool CanPlayCard(CardEntity card){
        PlayerEntity plr = game.plrs.GetPlayer(plr_priority);

        if(plr_priority != card.Plr_Id) return false;
        if(plr.Hand.Contains(card)) return false;
        if(card.Type != CardTypes.FastSpell) return false;
        if(card.Stats.Cost > plr.Mana) return false;

        return true;
    }

    public void DoCardEffects(){
        while(on_hold_card_effects.Count > 0){
            int i = on_hold_card_effects.Count-1;
            CardEffect a = on_hold_card_effects[i];
            a.Effect.Invoke();
            on_hold_card_effects.RemoveAt(i);
        }
    }

    public void CheckLegalPlays(){
        bool can = false;
        PlayerEntity plr = game.plrs.GetPlayer(plr_priority);
        for(int i = 0; i < plr.Hand.Count; i++){
            if(plr.Hand[i].Stats.Cost > plr.Mana) continue;
            if(plr.Hand[i].Type != CardTypes.FastSpell) continue;
            
            can = true;
            break;
        }

        if(!can) EndTurn();
    }

    public void AddEffect(CardEffect effect, bool progress){
        on_hold_card_effects.Add(effect);
        plr_priority = game.plrs.GetOtherPlayer(plr_priority).Id;
        if(progress) CheckLegalPlays();
    }
}