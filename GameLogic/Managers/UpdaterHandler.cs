public class UpdaterHandler(GameEntity game)
{
    public GameEntity game = game;
    public List<ClientUpdater> events = [];

    public List<Buff> applied_buffs = [];

    public void ChangeStats(Buff buff, int anim = 0)
    {
        StatUpdater clone = new(buff, DupeBuff(buff));
        clone.Action = "card buff change";
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

    public void ChangeCardLocation(CardLocations loc, CardLocations start, int card_id, int anim = 0)
    {
        CardLocationUpdater clone = new(loc, start, card_id);
        clone.Action = "card location change";
        events.Add(clone);
    }

    public void print()
    {
        Console.WriteLine("the new actions: ");
        int i = 0;
        foreach(ClientUpdater a in events){
            Console.WriteLine($"action {++i}: " + a.Action);
        }
    }

    public void UpdateClients()
    {
        print();
        ClientUpdateMessage clone = new(events);
        MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr1.Id), clone);
        MessageHandler.UpdateClient(ServerHandler.GetWSConnection(game.plrs.Plr2.Id), clone);
        events = [];
    }
}