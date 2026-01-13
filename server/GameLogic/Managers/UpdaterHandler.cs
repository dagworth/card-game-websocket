namespace server.GameLogic.Managers;

using server.GameLogic.Entities;
using server.GameLogic.Misc;
using server.ServerLogic;
using shared;
using shared.DTOs;

public class UpdaterHandler(GameEntity game) {
    public GameEntity game = game;
    public Dictionary<ClientUpdater, int> events = [];

    public List<Buff> applied_buffs = [];

    //buff is already dead (not attached to card) when here, so we need card_id to get passed
    public void ChangeStats(Buff buff, int card_id, int anim = 0) {
        BuffDTO buffdto = new(buff.Cost, buff.Cost_Fixed, buff.Attack, buff.Attack_Fixed, buff.Health, buff.Health_Fixed, buff.passives);
        StatUpdater clone = new(card_id, buffdto, DupeBuff(buff));
        CardEntity card = buff.card!;
        clone.Action = "card buff change";
        if (card.Location == CardLocations.Hand) {
            events[clone] = card.Plr_Id; //only person who has in hand can see change
        } else if (card.Location == CardLocations.Board || card.Location == CardLocations.Void) {
            events[clone] = -1; //both see board and void
        } else if (card.Location == CardLocations.Deck) {
            //do not update cus u cant see deck prob
        }
    }

    public void TookDamage(int damage, int anim = 0) {
        DamageUpdater clone = new(damage);
        clone.Action = "damage change";
        events[clone] = -1;
    }

    //this updater is different depending on where the change happens and who can see
    //plr_id is the player that needs this card information
    public void NewCard(CardEntity card, int anim = 0) {
        CardStatsDTO cardstatsdto = new(card.Stats.Cost, card.Stats.Health, card.Stats.Damaged, card.Stats.Attack, card.Stats.passives);
        CardEntityDTO cardentitydto = new(card.Id, card.Type, card.Location, card.Name,  cardstatsdto);
        NewCardUpdater clone = new(cardentitydto);
        clone.Action = "new card";
        events[clone] = card.Plr_Id;
    }

    public void EndTurn(int plr_id, int anim = 0) {
        TurnUpdater clone = new(plr_id);
        clone.Action = "turn ended";
        events[clone] = -1;
    }

    private bool DupeBuff(Buff buff) {
        if (applied_buffs.Contains(buff)) {
            applied_buffs.Remove(buff);
            return true;
        }
        applied_buffs.Add(buff);
        return false;
    }

    public void ChangeCardLocation(CardLocations new_loc, CardLocations start, int card_id, int anim = 0) {
        CardLocationUpdater clone = new(card_id, new_loc, start);
        clone.Action = $"card location change for {card_id}";
        events[clone] = -1;
    }

    // public void print(string be)
    // {
    //     Console.Write($"new actions {be}: ");
    //     int i = 0;
    //     foreach(KeyValuePair<ClientUpdater,int> a in events){
    //         Console.Write($"action {++i}: {a.Action} ");
    //     }
    //     Console.WriteLine();
    // }

    public void UpdateClients(string update_type = "none") {
        //print(update_type);
        ClientUpdateMessage clone0 = new() {
            Action = "clientupdate"
        };
        ClientUpdateMessage clone1 = new() {
            Action = "clientupdate"
        };
        foreach (KeyValuePair<ClientUpdater, int> updater in events) {
            if (updater.Value == -1) {
                clone0.Events.Add(updater.Key);
                clone1.Events.Add(updater.Key);
            }

            if (updater.Value == game.plrs.Plr0.Id) clone0.Events.Add(updater.Key);
            if (updater.Value == game.plrs.Plr1.Id) clone1.Events.Add(updater.Key);
        }
        if (clone0.Events.Count > 0) MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr0.Id), clone0);
        if (clone1.Events.Count > 0) MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr1.Id), clone1);
        events = [];
    }
}