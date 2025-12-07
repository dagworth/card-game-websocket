using Fleck;
using System.Text.Json;
using System.Net.WebSockets;
using System.Text;

static class Program {
    private static WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8181");
    
    static void Main() {
        server.Start(ws => {
            ws.OnOpen = () => ServerHandler.OnOpen(ws);
            ws.OnMessage = message => ServerHandler.OnMessage(ws, message);
            ws.OnClose = () => ServerHandler.OnClose(ws);
        });

        Console.WriteLine("server started");

        Task.Run(async () => await test());

        while(true){
            Console.ReadLine();
            PrintState.Print(GameManager.GetGame(0));
        }
    }

    private static async Task test()
    {
        void send(ClientWebSocket client, string message)
        {
            client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        await Task.Delay(200);

        var plr0 = new ClientWebSocket();
        await plr0.ConnectAsync(new Uri("ws://127.0.0.1:8181"), CancellationToken.None);
        var plr1 = new ClientWebSocket();
        await plr1.ConnectAsync(new Uri("ws://127.0.0.1:8181"), CancellationToken.None);

        await Task.Delay(200);

        send(plr0, "{ \"action\": \"join_waiting_queue\", \"player_id\": 0 }");
        send(plr1, "{ \"action\": \"join_waiting_queue\", \"player_id\": 1 }");

        await Task.Delay(100);

        send(plr1, "{ \"action\": \"play_card\", \"player_id\": 1, \"card_id\": 4 }");

        await Task.Delay(100);

        send(plr1, "{ \"action\": \"end_turn\", \"player_id\": 1 }");

    }
}