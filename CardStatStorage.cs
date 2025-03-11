public static class CardStatStorage {
    private static readonly Dictionary<string,CardData> data = new Dictionary<string,CardData>{
        {"Bob", new CardData {
                type = CardTypes.Minion,
                name = "Bob",
                description = "on play: he buffs all minions on board by +1/+1",
                cost = 1,
                health = 1,
                attack = 1,
                passives = [Passives.Charge],
                OnPlay = (owner, game) => {
                    foreach(CardStatus v in game.GetAllCardsOnBoard()){
                        v.attack += 1;
                        v.health += 1;
                        v.max_health += 1;
                    }
                }
            }
        },
        {"Alice", new CardData {
                type = CardTypes.Minion,
                name = "Alice",
                description = "on attack: she buffs herself by +1/+3",
                cost = 2,
                health = 4,
                attack = 2,
                passives = [Passives.Deadly],
                OnAttack = (owner, game) => {
                    owner.attack += 1;
                    owner.health += 3;
                    owner.max_health += 3;
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