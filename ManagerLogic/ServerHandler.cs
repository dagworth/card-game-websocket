using Fleck;
using System.Text.Json;

public static class ServerHandler {
    private static IWebSocketConnection? WaitingPlayer = null;
    private static Dictionary<IWebSocketConnection,int> plr_ids = new();
    private static int plr_counter = 0;

    public static void OnOpen(IWebSocketConnection ws){
        int id = plr_counter++;
        plr_ids[ws] = id;
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

        MessageHandler.ReadMessage(ws, message, data.Action);
    }

    public static void OnClose(IWebSocketConnection ws){
        Console.WriteLine($"{plr_ids[ws]} plr left");
    }

    public static void AddPlayerToQueue(IWebSocketConnection ws){
        if(WaitingPlayer != null){
            GameManager.CreateGame(plr_ids[ws], plr_ids[WaitingPlayer]);
            WaitingPlayer = null;
        } else {
            WaitingPlayer = ws;
        }
    }
}