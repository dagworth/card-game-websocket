using Godot;

public partial class TargetChoosingManager : Control {
    public static TargetChoosingManager Instance;

    private VBoxContainer option_box;

    public override void _Ready() {
        Instance = this;
        option_box = GetTree().Root.GetNode<VBoxContainer>("Main/UI/TargetChoice");
        option_box.Visible = false;
    }

    public void ShowChoices() {
        option_box.Visible = true;
        GameEvents.Instance.game_state = GameStates.Choosing;
    }
}