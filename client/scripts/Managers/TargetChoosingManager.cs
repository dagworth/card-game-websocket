using System.Collections.Generic;
using Godot;

public partial class TargetChoosingManager : Node2D {
	public static TargetChoosingManager Instance;

	private Panel choice_panel;
	private HBoxContainer option_box;
	private GameStateEnum old_state;

	public override void _Ready() {
		Instance = this;
		choice_panel = GetTree().Root.GetNode<Panel>("Main/UI/TargetChoice");
		option_box = choice_panel.GetNode<HBoxContainer>("MarginContainer/HBox");
		choice_panel.Visible = false;
	}

	public void ShowChoices(List<int> targets) {
		choice_panel.Visible = true;
		old_state = GameEvents.Instance.game_state;
		GameEvents.Instance.game_state = GameStateEnum.Choosing;
		foreach (int id in targets) {
            var button = new Button {
                Text = $"click me {id}"
            };
            option_box.AddChild(button);
			button.Pressed += () => ChooseChoice(id);
		}
	}

	public void ChooseChoice(int id) {
		choice_panel.Visible = false;
		MessageHandler.TargetChosen([id]);
		GameEvents.Instance.game_state = old_state;
	}
}
