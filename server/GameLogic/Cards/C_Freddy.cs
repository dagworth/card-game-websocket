using server.GameLogic.Entities;
using server.GameLogic;

public class C_Freddy : CardEffect {
    public override void OnAttack(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, freddy spawned");
        List<CardEntity> other = game.plrs.GetOtherPlayer(owner).Board;
        for(int i = other.Count - 1; i >= 0; i--){
            Buff b = other[i].AddTempBuff(new Buff{
                Attack = -2,
                Health = -2
            });

            game.MakeDelayedEffect(owner.Id, Delays.StartTurn, () => {
                b.Remove();
            });
        }
    }
}