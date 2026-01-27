using Godot;

public partial class PreviewManager : Control {
    public override void _Ready() {
        GameEvents.Instance.HandCardHover += ShowPreview;
        GameEvents.Instance.HandCardExit += HidePreview;
    }

    private void ShowPreview(HandCard card) {
        //change this preview into the card
        Visible = true;
    }

    private void HidePreview(HandCard card) => Visible = false;
}