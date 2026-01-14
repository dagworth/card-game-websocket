using server.GameLogic.Entities;
using server.GameLogic;

public class C_Dave : CardEffect {
    public override void OnSpawn(GameEntity game, PlayerEntity owner, CardEntity card) {
        Console.WriteLine("DEBUG, dave spawned");
        game.events.OnSacrifice += (sacrificed_card_id) => {
            if (game.cards.GetCard(sacrificed_card_id).Plr_Id != owner.Id) {
                card.AddPermBuff(new Buff {
                    Attack = 1,
                    Health = 1
                });
            }
        };
    }
}