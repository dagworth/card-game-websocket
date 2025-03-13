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
            GameManager.GetGame(0).PrintState();
        }
    }
}