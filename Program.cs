using Fleck;
using System.Text.Json;

static class Program {
    private static WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8181");
    
    static void Main() {
        server.Start(ws => {
            ws.OnOpen = () => ServerHandler.OnOpen(ws);
            ws.OnMessage = message => ServerHandler.OnMessage(ws, message);
            ws.OnClose = () => ServerHandler.OnClose(ws);
        });

        Console.WriteLine("server started");
        while(true){
            Console.ReadLine();
            PrintState.Print(GameManager.GetGame(0));
        }
    }
}