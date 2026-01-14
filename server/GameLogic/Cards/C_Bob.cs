using server.GameLogic.Entities;
using server.GameLogic;

public class C_Bob : CardEffect {
    public override void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, bob spawned");
        List<CardEntity> a = [];
        a.AddRange(game.plrs.Plr0.Board);
        a.AddRange(game.plrs.Plr1.Board);
        foreach(CardEntity v in a){
            v.AddPermBuff(new Buff{
                Attack = 1,
                Health = 1
            });
        }
    }
}