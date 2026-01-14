using server.GameLogic.Entities;
using server.GameLogic;

public class C_Eve : CardEffect {
    public override void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, eve spawned");
        game.QueryTargets(owner.Id,
            targets => {
                game.MakeCounterableEffect(owner.Id, card, () => {
                    game.events.SacrificeCard(targets[0]);
                    owner.DrawCard();
                    owner.DrawCard();
                });
            },

            new ChooseTargetsParams(owner.Id, [TargetTypes.YourUnits])
        );
    }
}