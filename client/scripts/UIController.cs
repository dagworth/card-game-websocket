using Godot;
using shared.DTOs;
using System;
using System.Collections.Generic;

public partial class UIController : Node2D {
    //card position
    private const int default_x_offset = 0;
    private const int default_y_offset = 0;
    private const int default_spacing = 165;
    private const float default_angle_increment = 3.5f;
    private const float default_card_scaling = 1.5f;
    private const int default_y_spread_increment = 10;

    //minimize card position
    private const int minimized_x_offset = 300;
    private const int minimized_y_offset = 150;
    private const int minimized_spacing = 90;
    private const float minimized_angle_increment = 0;
    private const float minimized_card_scaling = 1f;
    private const int minimized_y_spread_increment = 0;

    private const int screen_pos_minimize_threshold = 750;
    private const int place_minion_threshold = 700;

    //private const string card_data_path = "res://resources/card_data";
    private const string ui_card = "res://scenes/hand_card.tscn";

    private static List<Control> ui_cards = new();
    public static Control ui_hand;
    //[Export] public GameHandler gameHandler;
    //private GamePlayer plr;

    private static Control hover_card;
    private static Control drag_card;
    private static bool dragging;

    public override void _Ready() {
        ui_hand = GetTree().Root.GetNode<Control>("Main/UI/Hand");
    }


    public static void addCard(int card_id) {
        CardEntityDTO card = CardHandler.GetCard(card_id);
        CardDataDTO data = DataLoader.GetData(card.Name);
        //CardData base_stats = ResourceLoader.Load<CardData>($"{card_data_path}/{card_stats.name}.tres");

        PackedScene loaded_card = ResourceLoader.Load<PackedScene>(ui_card);

        Control clone = loaded_card.Instantiate() as Control;
        clone.GetNode<RichTextLabel>("NameLabel").Text = card.Name;
        clone.GetNode<RichTextLabel>("DescLabel").Text = data.Description;
        clone.GetNode<RichTextLabel>("AttackLabel").Text = card.Stats.Attack.ToString();
        clone.GetNode<RichTextLabel>("HealthLabel").Text = card.Stats.Health.ToString();
        clone.GetNode<RichTextLabel>("CostLabel").Text = card.Stats.Cost.ToString();
        //clone.GetNode<TextureRect>("ImageLabel").Texture = base_stats.image;

        ui_hand.AddChild(clone);

        //this part will be changed with an animation that makes it look less stupid
        clone.Position = new Vector2(1600, 800);
        ui_cards.Add(clone);

        clone.MouseEntered += () => onHover(clone);
        clone.MouseExited += () => onHoverExit(clone);
        updateCardPosition();
    }

    private static int x_offset = default_x_offset;
    private static int y_offset = default_y_offset;
    private static int spacing = default_spacing;
    private static float angle_increment = default_angle_increment;
    private static float card_scaling = default_card_scaling;
    private static int y_spread_increment = default_y_spread_increment;

    public static void updateCardPosition() {
        int count = ui_cards.Count;
        int side_index = count / 2;
        int hover_index = hover_card != null ? ui_cards.IndexOf(hover_card) : side_index;

        for (int i = 0; i < ui_cards.Count; i++) {
            Control card = ui_cards[i];

            float x = x_offset;
            float y = y_offset;

            float angle = 0;
            Vector2 scale = new Vector2(card_scaling, card_scaling);

            if (card == hover_card && !dragging) {
                x += 950 + (side_index - i) * -spacing;
                card.ZIndex = 1;
                scale = new Vector2(2.75f, 2.75f);
                y += 560;
            } else {
                x += 1000 + (side_index - i) * -spacing + (hover_index - i) * (float)Math.Pow(hover_index - i, 2) / 10;
                card.ZIndex = 0;
                y += 875 + Math.Abs(side_index - i) * y_spread_increment;
                angle = (side_index - i) * -angle_increment;
            }

            Vector2 pos = new Vector2(x, y);

            Tween tween = card.CreateTween();

            tween.TweenProperty(card, "position", pos, 0.2f)
                .SetTrans(Tween.TransitionType.Quad);

            tween.Parallel().TweenProperty(card, "rotation_degrees", angle, 0.2f)
                .SetTrans(Tween.TransitionType.Quad);

            tween.Parallel().TweenProperty(card, "scale", scale, 0.25f)
                .SetTrans(Tween.TransitionType.Quad);
        }
    }

    private static bool minimized = false;

    public static void onHover(Control card) {
        if (hover_card == null && !dragging && !minimized) {
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
                    ui_hand.GetParent().AddChild(drag_card);

                    dragging = true;
                    hover_card.Visible = false;

                    Tween tween = CreateTween();
                    tween.TweenProperty(drag_card, "rotation_degrees", 0f, 0.2f)
                        .SetTrans(Tween.TransitionType.Linear);

                    tween.Parallel().TweenProperty(drag_card, "scale", new Vector2(default_card_scaling, default_card_scaling), 0.1f)
                        .SetTrans(Tween.TransitionType.Linear);
                } else {
                    if (click.Position.Y < screen_pos_minimize_threshold) {
                        minimized = true;
                        y_spread_increment = minimized_y_spread_increment;
                        angle_increment = minimized_angle_increment;
                        spacing = minimized_spacing;
                        x_offset = minimized_x_offset;
                        y_offset = minimized_y_offset;
                        card_scaling = minimized_card_scaling;
                    } else {
                        minimized = false;
                        y_spread_increment = default_y_spread_increment;
                        angle_increment = default_angle_increment;
                        spacing = default_spacing;
                        x_offset = default_x_offset;
                        y_offset = default_y_offset;
                        card_scaling = default_card_scaling;
                    }
                }
                updateCardPosition();
            } else {
                if (dragging) {
                    if (click.Position.Y < place_minion_threshold) {
                        //we use hover_card here because drag_card is just a copy without the stats
                        //we will change this later so that the hover_card in cardstatus will be useful here

                        //play card

                        // int card_id = (hover_card as HandCard).stats.card_id;
                        // bool success = plr.playCard(card_id);

                        // if(success){
                        //     ui_cards.Remove(hover_card);
                        //     hover_card.QueueFree();
                        //     updateCardPosition();
                        // }
                    }
                    hover_card.Visible = true;
                    dragging = false;
                    hover_card = null;
                    drag_card.QueueFree();
                }
            }
        } else if (@event is InputEventMouseMotion move) {
            if (hover_card != null && !dragging) {
                if (move.Position.Y < place_minion_threshold) {
                    hover_card = null;
                    updateCardPosition();
                }
            }
        }
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
