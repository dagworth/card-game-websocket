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
                    foreach(CardStatus v in game.GetAllCardsOnBoard()){
                        v.ChangeStats(1,1);
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
                    card.ChangeStats(1,3);
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
                    //will do this after targetting
                    foreach(CardStatus v in owner.Board){
                        v.ChangeStats(1,22);
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
                    //after temporary stuff

                    // game.OnSacrifice += (sacrificed_card_id) => {
                    //     if(game.GetCard(sacrificed_card_id).plr_id != owner.id){
                    //         card.ChangeStats(1,1);
                    //     }
                    // };
                }
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
                            game.MakeCounterableEffect(owner.Id, input => {
                                game.SacrificeCard(input[0]);
                                owner.DrawCard();
                                owner.DrawCard();
                            }, targets);
                        },

                        new ChooseTargetsParams([..owner.Board]){
                            TargetType = TargetTypes.Unit,
                        }
                    );
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