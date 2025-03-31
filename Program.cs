using Fleck;

static class Program {
    private static WebSocketServer server = new WebSocketServer("ws://0.0.0.0:8181");
    
    static void Main() {
        server.Start(ws => {
            ws.OnOpen = () => ServerHandler.OnOpen(ws);
            ws.OnMessage = message => ServerHandler.OnMessage(ws, message);
            ws.OnClose = () => ServerHandler.OnClose(ws);
        });

        Console.WriteLine("server started");
        while(true){
            Console.ReadLine();

            //testing purposes
            PrintState(GameManager.GetGame(0));
        }
    }
    static void PrintState(Game game){
        Console.WriteLine($"\nplr {game.plr_turn} turn\n");
        Console.WriteLine($"gamestate {game.game_state}");
        Console.WriteLine($"plr1 health {game.plr1.health}");
        Console.WriteLine($"plr2 health {game.plr2.health}");

        Console.WriteLine($"plr {game.plr1.id} hand");
        foreach(CardStatus card in game.plr1.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {game.plr1.id} board");
        foreach(CardStatus card in game.plr1.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {game.plr2.id} hand");
        foreach(CardStatus card in game.plr2.Hand){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }

        Console.WriteLine($"\nplr {game.plr2.id} board");
        foreach(CardStatus card in game.plr2.Board){
            Console.Write($"({card.name} {card.attack}/{card.health} id: {card.card_id}) ");
        }
    }
}