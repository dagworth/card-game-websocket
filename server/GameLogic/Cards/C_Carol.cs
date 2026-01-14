using server.GameLogic.Entities;
using server.GameLogic;

public class C_Carol : CardEffect {
    public override void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, carol spawned");
        for(int i = owner.Board.Count - 1; i >= 0; i--){
            Buff b = owner.Board[i].AddTempBuff(new Buff{
                Attack = 1,
                Health = 1
            });

            game.MakeDelayedEffect(owner.Id, Delays.EndTurn, () => {
                b.Remove();
            });
        }
    }
}