using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class HandCardUpdater : Node2D {
    private static Control hand;
    private static List<Control> cards = new();
    private const int x_offset = 900;
    private const int y_offset = 925;
    private const int spacing = 160;
    private const float angle_increment = 2.5f;
    private const int y_spread_increment = 10;

    private const int place_minion_threshold = 700;

    private const string base_card = "res://scenes/hand_card.tscn";

    private static bool dragging;
    private static Control hover_card;
    private static Control drag_card;

    private static void updateCardPosition() {
        int count = cards.Count;
        int side_index = count / 2;

        for (int i = 0; i < cards.Count; i++) {
            Control card = cards[i];
            card.ZIndex = 0;

            float x = x_offset + (side_index - i) * -spacing;
            float y = y_offset + Math.Abs(side_index - i) * y_spread_increment;
            float angle = (side_index - i) * -angle_increment;

            Vector2 pos = new(x, y);

            Tween tween = card.CreateTween();

            tween.TweenProperty(card, "position", pos, 0.3f).SetTrans(Tween.TransitionType.Quad);
            tween.Parallel().TweenProperty(card, "rotation_degrees", angle, 0.3f).SetTrans(Tween.TransitionType.Quad);
        }
    }

    public static void addCard(int id) {
        CardEntityDTO card = CardHandler.GetCard(id);
        CardDataDTO data = DataLoader.GetData(card.Name);
        //CardData base_stats = ResourceLoader.Load<CardData>($"{card_data_path}/{card_stats.name}.tres");

        PackedScene loaded_card = ResourceLoader.Load<PackedScene>(base_card);

        Control clone = loaded_card.Instantiate() as Control;
        (clone as HandCard).data = card;
        clone.GetNode<RichTextLabel>("NameLabel").Text = card.Name;
        clone.GetNode<RichTextLabel>("DescLabel").Text = data.Description;
        clone.GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
        clone.GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();
        clone.GetNode<RichTextLabel>("CostLabel").Text = card.Stats.Cost.ToString();
        //clone.GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;

        hand.AddChild(clone);

        //this part will be changed with an animation that makes it look less stupid
        clone.Position = new Vector2(1600, 800);
        cards.Add(clone);

        clone.MouseEntered += () => onHover(clone);
        clone.MouseExited += () => onHoverExit(clone);
        updateCardPosition();
    }

    public static void onHover(Control card) {
        if (hover_card == null && !dragging) {
            hover_card = card;
            updateCardPosition();
        }
    }

    public static void onHoverExit(Control card) {
        if (hover_card == card && !dragging) {
            hover_card = null;
            updateCardPosition();
        }
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left) {
            if (click.IsEcho()) return;

            if (click.IsPressed()) {
                if (hover_card != null) {
                    drag_card = hover_card.Duplicate() as Control;
                    hand.GetParent().AddChild(drag_card);

                    dragging = true;
                    hover_card.Visible = false;

                    Tween tween = CreateTween();
                    tween.TweenProperty(drag_card, "rotation_degrees", 0f, 0.2f).SetTrans(Tween.TransitionType.Linear);
                }
            } else {
                if (dragging) {
                    if (click.Position.Y < place_minion_threshold) {
                        int card_id = (hover_card as HandCard).data.Id;
                        GD.Print(card_id);
                    }
                    hover_card.Visible = true;
                    dragging = false;
                    hover_card = null;
                    drag_card.QueueFree();
                }
            }
        } else if (@event is InputEventMouseMotion move) {
            if (drag_card != null && !dragging) {
                if (move.Position.Y < place_minion_threshold) {
                    drag_card = null;
                    updateCardPosition();
                }
            }
        }
    }
    
    public override void _Ready() {
        hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
    }

    public override void _Process(double delta) {
        if (dragging) {
            Vector2 pos = GetGlobalMousePosition();
            pos.X -= drag_card.Size.X;
            pos.Y -= drag_card.Size.Y;
            drag_card.Position = pos;
        }
    }
}
