using System.Globalization;

public static class CardStatStorage {
    private static readonly Dictionary<string,CardData> data = new Dictionary<string,CardData>{
        {"Bob", new CardData {
                type = CardTypes.Unit,
                tribes = [Tribes.Zombie],
                name = "Bob",
                description = "on play: he buffs all minions on board by +1/+1",
                cost = 1,
                health = 1,
                attack = 1,
                passives = [Passives.Charge],
                OnSpawn = (game, owner, card) => {
                    List<CardStatus> a = [];
                    a.AddRange(game.plrs.Plr1.Board);
                    a.AddRange(game.plrs.Plr2.Board);
                    foreach(CardStatus v in a){
                        v.MakeBuff(1,1);
                    }
                }
            }
        },
        {"Alice", new CardData {
                type = CardTypes.Unit,
                tribes = [Tribes.Skeleton],
                name = "Alice",
                description = "on attack: she buffs herself by +1/+3",
                cost = 2,
                health = 4,
                attack = 2,
                passives = [Passives.Deadly],
                OnAttack = (game, owner, card) => {
                    card.MakeBuff(1,3);
                }
            }
        },
        {"Carol", new CardData {
                type = CardTypes.Unit,
                tribes = [],
                name = "Carol",
                description = "on play: buffs ally units by +1/+22 this turn",
                cost = 1,
                health = 2,
                attack = 6,
                passives = [Passives.Charge],
                OnSpawn = (game, owner, card) => {
                    foreach(CardStatus v in owner.Board){
                        v.MakeBuff(1,22);
                    }
                }
            }
        },
        {"Dave", new CardData {
                type = CardTypes.Unit,
                tribes = [Tribes.Creeper],
                name = "Dave",
                description = "on enemy sacrifice: gain +1/+1",
                cost = 1,
                health = 2,
                attack = 6,
                passives = [Passives.Charge],
                custom_effects = (game, owner, card) => {
                    game.events.OnSacrifice += (sacrificed_card_id) => {
                        if(card.Location == CardLocations.Board){
                            if(game.cards.GetCard(sacrificed_card_id).Plr_Id != owner.Id){
                                card.MakeBuff(1,1);
                            }
                        }
                    };
                },
            }
        },
        {"Eve", new CardData {
                type = CardTypes.Unit,
                tribes = [],
                name = "Eve",
                description = "on spawn: choose an allied unit to sacrifice to draw 2 cards",
                cost = 3,
                health = 1,
                attack = 3,
                passives = [],
                OnSpawn = (game, owner, card) => {
                    game.QueryTargets(owner.Id,
                        targets => {
                            game.MakeCounterableEffect(owner.Id, card, () => {
                                game.events.SacrificeCard(targets[0]);
                                owner.DrawCard();
                                owner.DrawCard();
                            });
                        },

                        new ChooseTargetsParams([..owner.Board]){
                            TargetType = TargetTypes.Unit,
                        }
                    );
                }
            }
        },
        {"Haunting Scream", new CardData {
                type = CardTypes.Spell,
                name = "Haunting Scream",
                description = "choose a unit that costs 5 or less from your void. Bring it back from the void and give it flying and charge. Sacrifice it at the end of the turn",
                cost = 3,
                OnPlay = (game, owner, card, t) => {
                    game.MakeCounterableEffect(owner.Id, card, () => {
                        game.QueryTargets(owner.Id,
                            targets => {
                                CardStatus guy = game.cards.GetCard(targets[0]);
                                game.events.BringOutVoid(targets[0]);
                                guy.GivePassive(Passives.Charge);
                                guy.GivePassive(Passives.Flying);
                            },

                            new ChooseTargetsParams([..owner.Void]){
                                TargetType = TargetTypes.Unit,
                                TargetMaxCost = 5,
                            }
                        );
                    });
                }
            }
        }
    };

    public static CardData GetCardData(string card_name){
        if(data.ContainsKey(card_name)){
            return data[card_name];
        } else {
            Console.WriteLine($"'{card_name}' card data doesnt exist dumbo");
            return new CardData();
        }
    } 
}