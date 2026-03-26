using Godot;

public partial class DefendManager : Node2D {
	public static DefendManager Instance;

    private int current_defender;

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
            //make sure its a real card
            BoardCard card = BoardCardManager.Instance.hover_card;
            if (card == null) return;

			if (click.Pressed) {
				if(card.card_entity.plr_id != ClientHandler.plr_id) return; //not ur card
				if(GameEvents.Instance.game_state != GameStateEnum.Regular && GameEvents.Instance.game_state != GameStateEnum.Attacking) return;

				GD.Print("start defend");

				current_defender = card.card_entity.id;
                return;
			} else if (current_defender != -1) {
				GD.Print("almost success defend");
                if(card.card_entity.plr_id == ClientHandler.plr_id) return; //is ur card
				GD.Print("2 almost defend");
				if(GameEvents.Instance.game_state != GameStateEnum.Defending) return;

				GD.Print("success defend");

                MessageHandler.ToggleDefend(current_defender, card.card_entity.id);
            }
            current_defender = -1;
		}
	}

	public override void _Ready() {
		Instance = this;
	}
}
