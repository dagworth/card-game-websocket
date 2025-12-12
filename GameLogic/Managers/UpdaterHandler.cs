public class UpdaterHandler(GameEntity game)
{
    public GameEntity game = game;
    public List<ClientUpdater> events = [];

    public List<Buff> applied_buffs = [];

    public void ChangeStats(Buff buff, int anim = 0)
    {
        StatUpdater clone = new(buff, DupeBuff(buff));
        //buff is already dead (not attached to card) when here
        clone.Action = "card buff change";
        events.Add(clone);
    }

    public void TookDamage(int damage, int anim = 0)
    {
        DamageUpdater clone = new(damage);
        clone.Action = "damage change";
        events.Add(clone);
    }

    public void EndTurn(int plr_id, int anim = 0)
    {
        TurnUpdater clone = new(plr_id);
        clone.Action = "turn ended";
        events.Add(clone);
    }

    private bool DupeBuff(Buff buff)
    {
        if (applied_buffs.Contains(buff))
        {
            applied_buffs.Remove(buff);
            return true;
        }
        applied_buffs.Add(buff);
        return false;
    }

    public void ChangeCardLocation(CardLocations new_loc, CardLocations start, int card_id, int anim = 0)
    {
        CardLocationUpdater clone = new(new_loc, start, card_id);
        clone.Action = $"card location change for {card_id}";
        events.Add(clone);
    }

    public void print(string be)
    {
        Console.Write($"new actions {be}: ");
        int i = 0;
        foreach(ClientUpdater a in events){
            Console.Write($"action {++i}: {a.Action} ");
        }
        Console.WriteLine();
    }

    public void UpdateClients(string update_type)
    {
        print(update_type);
        ClientUpdateMessage clone = new(events);
        MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr0.Id), clone);
        MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr1.Id), clone);
        events = [];
    }
}