//purely to debug

public static class PrintState{
    public static void Print(Game game){
        Console.Write($"plr {game.Plr_Turn} turn");

        Console.Write($"\n\n{game.plrs.Plr1.Id}: {game.plrs.Plr1.Health} hp, {game.plrs.Plr1.Mana} mana");
        Console.Write($"\n{game.plrs.Plr2.Id}: {game.plrs.Plr2.Health} hp, {game.plrs.Plr2.Mana} mana");

        Console.Write("\n\ndescription: ");

        if(game.Game_State is DefendingState){
            Console.Write($"waiting for plr {game.plrs.GetOtherPlayer(game.Plr_Turn).Id} to defend");
        } else if(game.Game_State is AttackingState){
            Console.Write($"waiting for plr {game.Plr_Turn} to attack");
        } else if(game.Game_State is RegularState){
            Console.Write($"waiting for plr {game.Plr_Turn} to play cards");
        } else if(game.Game_State is ChoosingState a){
            Console.Write($"waiting for plr {a.plr_choosing} to choose thing");
        } else if(game.Game_State is PriorityState b){
            Console.Write($"waiting for plr {b.plr_priority} to priority choose");
        }

        Console.Write($"\n\n{game.plrs.Plr1.Id} hand: ");
        foreach(CardStatus card in game.plrs.Plr1.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.plrs.Plr1.Id} board: ");
        foreach(CardStatus card in game.plrs.Plr1.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.plrs.Plr1.Id} void: ");
        foreach(CardStatus card in game.plrs.Plr1.Void){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n\n{game.plrs.Plr2.Id} hand: ");
        foreach(CardStatus card in game.plrs.Plr2.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.plrs.Plr2.Id} board: ");
        foreach(CardStatus card in game.plrs.Plr2.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.Write($"\n{game.plrs.Plr2.Id} void: ");
        foreach(CardStatus card in game.plrs.Plr2.Void){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }
    }
}