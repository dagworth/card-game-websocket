using server.GameLogic.Entities;
using server.GameLogic;
using server.GameLogic.Interfaces;

public class C_Haunting_Scream : CardEffect {
    public override void OnPlay(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, played haunting");
        game.MakeCounterableEffect(owner.Id, card, () => {
            game.QueryTargets(owner.Id,
                targets => {
                    CardEntity guy = game.cards.GetCard(targets[0]);
                    game.events.BringOutVoid(targets[0]);
                    guy.AddPermBuff(new Buff{
                        passives = [Passives.Flying, Passives.Charge]
                    });
                    game.MakeDelayedEffect(owner.Id, Delays.EndTurn, () => {
                        game.events.SacrificeCard(guy.Id);
                    });
                },

                new ChooseTargetsParams(owner.Id, [TargetTypes.YourUnitsInVoid]){
                    TargetMaxCost = 5,
                }
            );
        });
    }
}