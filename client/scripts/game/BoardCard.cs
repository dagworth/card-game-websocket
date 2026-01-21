using Godot;
using shared.DTOs;

public partial class BoardCard : Node2D, Hoverable {
	public CardEntityDTO data;
	public Control hover_card;
}
