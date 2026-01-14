using server.GameLogic.Entities;
using server.GameLogic;
using server.GameLogic.Interfaces;

public class C_Gary : CardEffect {
    public override void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, gary spawned");
        game.QueryTargets(owner.Id,
            targets => {
                game.MakeCounterableEffect(owner.Id, card, () => {
                    IDamageable guy = game.GetTarget(owner.Id, targets[0]);
                    guy.TakeDamage(3);
                });
            },

            new ChooseTargetsParams(owner.Id, [TargetTypes.Enemies])
        );
    }
}