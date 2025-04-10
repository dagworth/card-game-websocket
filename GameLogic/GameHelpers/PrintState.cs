public static class PrintState{
    public static void Print(Game game){
        Console.Write($"plr {game.Plr_Turn} turn");

        Console.Write($"\n\n{game.Plr1.Id}: {game.Plr1.Health} hp, {game.Plr1.Mana} mana");
        Console.Write($"\n{game.Plr2.Id}: {game.Plr2.Health} hp, {game.Plr2.Mana} mana");

        Console.Write("\n\ndescription: ");

        if(game.GetGameState() is DefendingState){
            Console.Write($"waiting for plr {game.GetOtherPlayer(game.Plr_Turn).Id} to defend");
        } else if(game.GetGameState() is AttackingState){
            Console.Write($"waiting for plr {game.Plr_Turn} to attack");
        } else if(game.GetGameState() is RegularState){
            Console.Write($"waiting for plr {game.Plr_Turn} to play cards");
        }

        Console.Write($"\n\n{game.Plr1.Id} hand: ");
        foreach(CardStatus card in game.Plr1.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.Plr1.Id} board: ");
        foreach(CardStatus card in game.Plr1.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.Plr1.Id} void: ");
        foreach(CardStatus card in game.Plr1.Void){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n\n{game.Plr2.Id} hand: ");
        foreach(CardStatus card in game.Plr2.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.Plr2.Id} board: ");
        foreach(CardStatus card in game.Plr2.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.Plr2.Id} void: ");
        foreach(CardStatus card in game.Plr2.Void){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }
    }
}