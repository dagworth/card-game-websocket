using server.GameLogic.Entities;
using server.GameLogic;

public class C_Alice : CardEffect {
    public override void OnAttack(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, alice attacked");
        card.AddPermBuff(new Buff {
            Attack = 1,
            Health = 3
        });
    }
}