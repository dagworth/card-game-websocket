using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class GameEvents : Node {
    public static GameEvents Instance { get; private set; }

    public GameStates game_state { get; set; } = GameStates.Regular;

    [Signal] public delegate void HandCardHoverEventHandler(HandCard card);
    [Signal] public delegate void HandCardExitEventHandler(HandCard card);

    [Signal] public delegate void BoardCardHoverEventHandler(BoardCard card);
    [Signal] public delegate void BoardCardExitEventHandler(BoardCard card);
    
    public override void _Ready() {
        Instance = this;
    }
}