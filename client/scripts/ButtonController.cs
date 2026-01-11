using Godot;

public partial class ButtonController : Node {
    public override void _Ready() {
        Button join_button = GetNode<Button>("../UI/JoinQueue");
        Button end_button = GetNode<Button>("../UI/EndTurn");

        join_button.Pressed += joinqueue;
        end_button.Pressed += endturn;
    }

    public void joinqueue() {
        MessageHandler.SendJoinQueue();
    }

    public void endturn() {
        MessageHandler.SendEndTurn();
    }
}