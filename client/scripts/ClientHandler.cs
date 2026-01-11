using Godot;
using System;
using System.Text;

public partial class ClientHandler : Node {
	public static int plr_id = -1;
	private static WebSocketPeer client;
	private static WebSocketPeer.State state;

	public override void _Ready() {
		client = new WebSocketPeer();
		client.ConnectToUrl("ws://127.0.0.1:8181");
		//UIController.addCard(1);
	}

	public override void _Process(double delta) {
		client.Poll();
		WebSocketPeer.State current_state = client.GetReadyState();

		if (state != current_state) {
			GD.Print($"ws changed to: {current_state}");
			state = current_state;
		}

		if (state == WebSocketPeer.State.Open) {
			if (client.GetAvailablePacketCount() > 0) {
				string message = Encoding.UTF8.GetString(client.GetPacket());
				GotMessage(message);
			}
		}
	}

	public void GotMessage(string message) {
		if (plr_id == -1) {
			plr_id = MessageHandler.ExecuteMessage(message);
			GD.Print($"this client's id: {plr_id}");
		} else {
			MessageHandler.ExecuteMessage(message);
		}
	}


	public static void SendMessage(string message) {
		if (IsConnected()) {
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, WebSocketPeer.WriteMode.Text);
			GD.Print($"Sent: {message}");
		}
	}

	public static bool IsConnected() {
		return client.GetReadyState() == WebSocketPeer.State.Open;
	}
}
