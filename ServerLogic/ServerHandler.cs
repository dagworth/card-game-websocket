using Fleck;
using System.Text.Json;

public static class ServerHandler {
    private static IWebSocketConnection? WaitingPlayer = null;
    private static readonly Dictionary<IWebSocketConnection,int> plr_ids = [];
    private static readonly Dictionary<int,IWebSocketConnection> plr_ws = [];
    private static int plr_counter = 0;

    public static void OnOpen(IWebSocketConnection ws){
        int id = plr_counter++;
        plr_ids[ws] = id;
        plr_ws[id] = ws;
        ws.Send(id.ToString());
    }

    public static void OnMessage(IWebSocketConnection ws, string message){
        Message? data = JsonSerializer.Deserialize<Message>(message);

        if(data == null){
            Console.WriteLine($"message went wrong: {message}");
            return;
        }
        
        if(data.PlayerId != plr_ids[ws]){
            Console.WriteLine($"player isnt who they say they are: {message}");
            return;
        }

        MessageHandler.ReadMessage(ws, data.PlayerId, message, data.Action);
    }

    public static void OnClose(IWebSocketConnection ws){
        Console.WriteLine($"plr {plr_ids[ws]} disconnected");
    }

    public static void AddPlayerToQueue(IWebSocketConnection ws){
        if(WaitingPlayer != null){
            GameManager.CreateGame(plr_ids[ws], plr_ids[WaitingPlayer]);
            WaitingPlayer = null;
        } else {
            WaitingPlayer = ws;
        }
    }

    public static IWebSocketConnection GetWSConnection(int plr_id){
        return plr_ws[plr_id];
    }
}