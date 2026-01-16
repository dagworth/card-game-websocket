namespace server.ServerLogic;

using shared;

using Fleck;
using System.Text.Json;
using System.Collections.Concurrent;

public static class ServerHandler {
    private static IWebSocketConnection? WaitingPlayer = null;
    private static readonly ConcurrentDictionary<IWebSocketConnection, int> plr_ids = [];
    private static readonly ConcurrentDictionary<int, IWebSocketConnection> plr_ws = [];
    private static int plr_counter = 10;

    public static void OnOpen(IWebSocketConnection ws) {
        int id = Interlocked.Increment(ref plr_counter);

        plr_ids[ws] = id;
        plr_ws[id] = ws;
        
        ws.Send(JsonSerializer.Serialize<ServerEvent>(new InformId() {
            PlayerId = id
        }));
    }

    public static void OnMessage(IWebSocketConnection ws, string message) {
        ClientRequest? data = JsonSerializer.Deserialize<ClientRequest>(message);

        if (data == null) {
            Console.WriteLine($"message went wrong: {message}");
            return;
        }

        if (data.PlayerId != plr_ids[ws]) {
            Console.WriteLine($"player isnt who they say they are: {message}");
            return;
        }

        MessageHandler.ReadMessage(ws, data.PlayerId, message);
    }

    public static void OnClose(IWebSocketConnection ws) {
        Console.WriteLine($"plr {plr_ids[ws]} disconnected");
    }

    public static void AddPlayerToQueue(IWebSocketConnection ws) {
        if (WaitingPlayer != null) {
            GameManager.CreateGame(plr_ids[ws], plr_ids[WaitingPlayer]);
            WaitingPlayer = null;
        } else {
            WaitingPlayer = ws;
        }
    }

    public static IWebSocketConnection GetWSConnection(int plr_id) {
        return plr_ws[plr_id];
    }
}