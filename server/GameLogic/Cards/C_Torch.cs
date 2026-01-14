using server.GameLogic.Entities;
using server.GameLogic;
using server.GameLogic.Interfaces;

public class C_Torch : CardEffect {
    public override void OnPlay(GameEntity game, PlayerEntity owner, CardEntity card) {
        game.MakeCounterableEffect(owner.Id, card, () => {
            game.QueryTargets(owner.Id,
                targets => {
                    CardEntity guy = game.cards.GetCard(targets[0]);
                    guy.TakeDamage(3);
                },

                new ChooseTargetsParams(owner.Id, [TargetTypes.Enemies]) { }
            );
        });
    }
}