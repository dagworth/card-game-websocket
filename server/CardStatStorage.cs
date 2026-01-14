// namespace server.GameLogic.Misc;

// using server.GameLogic.Entities;
// using server.GameLogic.Interfaces;

// public static class CardStatStoragenew {
//     private static readonly Dictionary<string, CardData> data = new Dictionary<string, CardData>{
// {"Alice", new CardData {
//         type = CardTypes.Unit,
//         tribes = [Tribes.Skeleton],
//         description = "on attack: she buffs herself by +1/+3",
//         cost = 2,
//         health = 4,
//         attack = 2,
//         passives = [Passives.Deadly],
//         OnAttack = (game, owner, card) => {
//             card.AddPermBuff(new Buff {
//                 Attack = 1,
//                 Health = 3
//             });
//         }
//     }
// },
// {"Bob", new CardData {
//         type = CardTypes.Unit,
//         tribes = [Tribes.Zombie],
//         description = "on play: he buffs all minions on board by +1/+1",
//         cost = 1,
//         health = 1,
//         attack = 1,
//         passives = [Passives.Charge],
//         OnSpawn = (game, owner, card) => {
//             List<CardEntity> a = [];
//             a.AddRange(game.plrs.Plr0.Board);
//             a.AddRange(game.plrs.Plr1.Board);
//             foreach(CardEntity v in a){
//                 v.AddPermBuff(new Buff{
//                     Attack = 1,
//                     Health = 1
//                 });
//             }
//         }
//     }
// },
// {"Carol", new CardData {
//         type = CardTypes.Unit,
//         tribes = [],
//         description = "on play: buffs allied units by +1/+22 this turn",
//         cost = 1,
//         health = 2,
//         attack = 6,
//         passives = [Passives.Charge],
//         OnSpawn = (game, owner, card) => {
//             for(int i = owner.Board.Count - 1; i >= 0; i--){
//                 Buff b = owner.Board[i].AddTempBuff(new Buff{
//                     Attack = 1,
//                     Health = 1
//                 });

//                 game.MakeDelayedEffect(owner.Id, Delays.EndTurn, () => {
//                     b.Remove();
//                 });
//             }
//         }
//     }
// },
// {"Dave", new CardData {
//         type = CardTypes.Unit,
//         tribes = [Tribes.Creeper],
//         description = "on enemy sacrifice: gain +1/+1",
//         cost = 1,
//         health = 2,
//         attack = 6,
//         passives = [Passives.Charge],
//         custom_effects = (game, owner, card) => {
//             game.events.OnSacrifice += (sacrificed_card_id) => {
//                 if(card.Location == CardLocations.Board){
//                     if(game.cards.GetCard(sacrificed_card_id).Plr_Id != owner.Id){
//                         card.AddPermBuff(new Buff{
//                             Attack = 1,
//                             Health = 1
//                         });
//                     }
//                 }
//             };
//         },
//     }
// },
// {"Eve", new CardData {
//         type = CardTypes.Unit,
//         tribes = [],
//         description = "on spawn: choose an allied unit to sacrifice to draw 2 cards",
//         cost = 3,
//         health = 1,
//         attack = 3,
//         passives = [],
//         OnSpawn = (game, owner, card) => {
//             game.QueryTargets(owner.Id,
//                 targets => {
//                     game.MakeCounterableEffect(owner.Id, card, () => {
//                         game.events.SacrificeCard(targets[0]);
//                         owner.DrawCard();
//                         owner.DrawCard();
//                     });
//                 },

//                 new ChooseTargetsParams(owner.Id, [TargetTypes.YourUnits])
//             );
//         }
//     }
// },
// {"Freddy", new CardData {
//         type = CardTypes.Unit,
//         tribes = [Tribes.Skeleton],
//         description = "on spawn: all enemy units get -2/-2 until the start of your next turn",
//         cost = 3,
//         health = 1,
//         attack = 3,
//         passives = [],
//         OnSpawn = (game, owner, card) => {
//             List<CardEntity> other = game.plrs.GetOtherPlayer(owner).Board;
//             for(int i = other.Count - 1; i >= 0; i--){
//                 Buff b = other[i].AddTempBuff(new Buff{
//                     Attack = -2,
//                     Health = -2
//                 });

//                 game.MakeDelayedEffect(owner.Id, Delays.StartTurn, () => {
//                     b.Remove();
//                 });
//             }
//         }
//     }
// },
// {"Gary", new CardData {
//         type = CardTypes.Unit,
//         tribes = [Tribes.Creeper],
//         description = "on spawn: choose an enemy to take 3 damage",
//         cost = 3,
//         health = 1,
//         attack = 3,
//         passives = [],
//         OnSpawn = (game, owner, card) => {
//             game.QueryTargets(owner.Id,
//                 targets => {
//                     game.MakeCounterableEffect(owner.Id, card, () => {
//                         IDamageable guy = game.GetTarget(owner.Id, targets[0]);
//                         guy.TakeDamage(3);
//                     });
//                 },

//                 new ChooseTargetsParams(owner.Id, [TargetTypes.Enemies])
//             );
//         }
//     }
// },
// {"Haunting Scream", new CardData {
//         type = CardTypes.Spell,
//         description = "choose a unit that costs 5 or less from your void. Bring it back from the void and give it flying and charge. Sacrifice it at the end of the turn",
//         cost = 3,
//         OnPlay = (game, owner, card, t) => {
//             game.MakeCounterableEffect(owner.Id, card, () => {
//                 game.QueryTargets(owner.Id,
//                     targets => {
//                         CardEntity guy = game.cards.GetCard(targets[0]);
//                         game.events.BringOutVoid(targets[0]);
//                         guy.AddPermBuff(new Buff{
//                             passives = [Passives.Flying, Passives.Charge]
//                         });
//                         game.MakeDelayedEffect(owner.Id, Delays.EndTurn, () => {
//                             game.events.SacrificeCard(guy.Id);
//                         });
//                     },

//                     new ChooseTargetsParams(owner.Id, [TargetTypes.YourUnitsInVoid]){
//                         TargetMaxCost = 5,
//                     }
//                 );
//             });
//         }
//     }
// },
// {"Torch", new CardData {
//         type = CardTypes.Spell,
//         description = "deal 3 damage",
//         cost = 1,
//         OnPlay = (game, owner, card, t) => {

//         }
//     }
// }
//     };

//     public static CardData GetCardData(string card_name) {
//         if (data.ContainsKey(card_name)) {
//             return data[card_name];
//         } else {
//             Console.WriteLine($"'{card_name}' card data doesnt exist dumbo");
//             return new CardData();
//         }
//     }
// }