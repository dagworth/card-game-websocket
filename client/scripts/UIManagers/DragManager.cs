using Godot;

public partial class DragManager : Node2D {
    private Control hand;
    private HandCard original;
    private HandCard drag_card;

    private const int place_minion_threshold = 700;

    public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
			if (click.Pressed) {
                if(HandCardManager.Instance.hover_card == null) return;
                original = HandCardManager.Instance.hover_card;
				OnDragStarted(original);
			} else if (!click.Pressed) {
    			OnDragEnded(original,click.Position);
			}
		}
	}

    private void OnDragStarted(HandCard card) {
        original = card;
        drag_card = original.Duplicate() as HandCard;
        drag_card.RotationDegrees = 0;
        original.Visible = false;
        hand.AddChild(drag_card);
        
        //Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public override void _Process(double delta) {
        if (drag_card != null) {
            drag_card.GlobalPosition = GetGlobalMousePosition() - (drag_card.Size / 2);
        }
    }

    public override void _Ready() {
        hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
    }

    private void OnDragEnded(HandCard card, Vector2 pos) {
        if (pos.Y < place_minion_threshold) {
            MessageHandler.PlayCard(original.card_entity.id);
        }

        drag_card?.QueueFree();
        drag_card = null;
        original.Visible = true;
        original = null;
        //Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}