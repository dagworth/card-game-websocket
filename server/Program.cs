using Fleck;
using System.Text.Json;
using System.Net.WebSockets;
using System.Text;
using System.Runtime.CompilerServices;

using server.ServerLogic;
using shared;

public static class Program {
    private static WebSocketServer server = new("ws://127.0.0.1:8181");

    static void Main() {
        RuntimeHelpers.RunClassConstructor(typeof(DataLogicLoader).TypeHandle);

        server.Start(ws => {
            ws.OnOpen = () => ServerHandler.OnOpen(ws);
            ws.OnMessage = message => ServerHandler.OnMessage(ws, message);
            ws.OnClose = () => ServerHandler.OnClose(ws);
        });

        //Test().Wait();

        while (true) {
            string? input = Console.ReadLine();
            PrintState.Print(GameManager.GetGame(0));
        }
    }

    private static async Task Test() {
        TextWriter original = Console.Out;
        TextWriter file = File.CreateText("log.txt");
        Console.SetOut(file);
        original.WriteLine("tests starting");
        await simulate_clients();
        PrintState.Print(GameManager.GetGame(0));
        Console.SetOut(original);
        file.Close();
        original.WriteLine("test done, file closed");
    }

    private static async Task simulate_clients() {
        void send(ClientWebSocket client, string message) {
            client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        int plr0_id = -1;
        int plr1_id = -1;

        await Task.Delay(250);

        ClientWebSocket plr0 = new ClientWebSocket();
        await plr0.ConnectAsync(new Uri("ws://127.0.0.1:8181"), CancellationToken.None);

        await Task.Delay(250);

        ClientWebSocket plr1 = new ClientWebSocket();
        await plr1.ConnectAsync(new Uri("ws://127.0.0.1:8181"), CancellationToken.None);

        await Task.Delay(250);

        async Task PrintMessages(ClientWebSocket ws, int id) {
            var buffer = new byte[1024];
            while (true) {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (id == 0 && plr0_id == -1) {
                    InformId a = JsonSerializer.Deserialize<InformId>(message)!;
                    plr0_id = a.PlayerId;
                }
                if (id == 1 && plr1_id == -1) {
                    InformId a = JsonSerializer.Deserialize<InformId>(message)!;
                    plr1_id = a.PlayerId;
                }
                Console.WriteLine($"plr{id} recieved: {message}");
            }
        }

        _ = PrintMessages(plr0, 0);
        _ = PrintMessages(plr1, 1);

        while (plr0_id == -1 || plr1_id == -1) {
            await Task.Delay(10);
        }

        send(plr0, $"{{ \"$type\": \"joinqueue\", \"player_id\": {plr0_id} }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"joinqueue\", \"player_id\": {plr1_id} }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"playcard\", \"player_id\": {plr1_id} , \"card_id\": 4 }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"endturn\", \"player_id\": {plr1_id} }}");
        await Task.Delay(100);
        send(plr0, $"{{ \"$type\": \"playcard\", \"player_id\": {plr0_id} , \"card_id\": 19 }}");
        await Task.Delay(100);
        send(plr0, $"{{ \"$type\": \"targetschoice\", \"player_id\": {plr0_id} , \"targets\": [4] }}");
        await Task.Delay(100);
        send(plr0, $"{{ \"$type\": \"endturn\", \"player_id\": {plr0_id} }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"playcard\", \"player_id\": {plr1_id} , \"card_id\": 0 }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"targetschoice\", \"player_id\": {plr1_id} , \"targets\": [4] }}");
        await Task.Delay(100);
        send(plr1, $"{{ \"$type\": \"endturn\", \"player_id\": {plr1_id} }}");
    }
}