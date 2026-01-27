using Godot;

public partial class AttackManager : Node2D {
    private static int units_attacking = 0;

    public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
			if (click.Pressed) {
                if (BoardCardManager.Instance.hover_card == null) {
                    GD.Print("null");
                    return;
                }
                if(BoardCardManager.Instance.hover_card.card_entity.plr_id != ClientHandler.plr_id) return; //not ur card
                MessageHandler.ToggleAttack(BoardCardManager.Instance.hover_card.card_entity.id);

                bool attacking = BoardCardManager.Instance.hover_card.ToggleAttack();
				units_attacking += attacking ? 1 : -1;

                if(units_attacking > 0) {
					GetTree().Root.GetNode<Button>("Main/UI/EndTurn").Text = "Attack";
				} else {
					GetTree().Root.GetNode<Button>("Main/UI/EndTurn").Text = "End Turn";
				}
			}
		}
	}
}