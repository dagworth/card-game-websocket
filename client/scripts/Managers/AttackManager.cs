using Godot;

public partial class AttackManager : Node2D {
	public static AttackManager Instance;
	private static int units_attacking = 0;
	private Button end_turn_button;

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
			if (click.Pressed) {
				BoardCard card = BoardCardManager.Instance.hover_card;
				if (card == null) return;
				if(card.card_entity.plr_id != ClientHandler.plr_id) return; //not ur card
				if(GameEvents.Instance.game_state != GameStates.Regular && GameEvents.Instance.game_state != GameStates.Attacking) return;

				MessageHandler.ToggleAttack(card.card_entity.id);
			}
		}
	}

	public void ToggleAttack(int card_id, bool status) {
		BoardCard card = CardHandler.GetCard(card_id).board_card;
		card.ToggleAttack(status);
		units_attacking += status ? 1 : -1;

		//i need to make it so the client can tell if they have thturn or not
		//so that attack isnt on the defending player's button

		if(units_attacking > 0) {
			GameEvents.Instance.game_state = GameStates.Attacking;
			end_turn_button.Text = "Attack";
		} else {
			GameEvents.Instance.game_state = GameStates.Regular;
			end_turn_button.Text = "End Turn";
		}
	}

	public override void _Ready() {
		Instance = this;
		end_turn_button = GetTree().Root.GetNode<Button>("Main/UI/EndTurn");
	}
}
