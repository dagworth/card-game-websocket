using Godot;

public partial class PreviewManager : Node2D {
	public override void _Ready() {
		GameEvents.Instance.HandCardHover += ShowPreview;
		GameEvents.Instance.HandCardExit += HidePreview;

		GameEvents.Instance.BoardCardHover += ShowPreview;
		GameEvents.Instance.BoardCardExit += HidePreview;
	}

	private void HidePreview(IHoverable card) => card.card_entity.preview_card.Visible = false;
	private void ShowPreview(IHoverable card) {
		GD.Print(card);
		GD.Print(card.card_entity);
		GD.Print(card.card_entity.preview_card);
		card.card_entity.preview_card.Visible = true;
	}
}
	
