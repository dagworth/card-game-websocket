using Godot;

public partial class PlayerHealthMana : Node2D {
    public static PlayerHealthMana Instance;
    private int your_hp = 35;
    private int enemy_hp = 35;

    private int your_mana = 0;
    private int enemy_mana = 0;

    private int your_max_mana = 0;
    private int enemy_max_mana = 0;

    private Label yourhplabel;
    private Label yourmanalabel;
    private Label enemyhplabel;
    private Label enemymanalabel;


    public void ChangeMana(int plr_id, int max, int value) {
        if(plr_id == ClientHandler.plr_id) {
            your_mana = value;
            your_max_mana = max;
        } else {
            enemy_mana = value;
            enemy_max_mana = max;
        }

        Update();
    }

    public void TookDamage(int plr_id, int damage) {
        if(plr_id == ClientHandler.plr_id) {
            your_hp -= damage;
        } else {
            enemy_hp -= damage;
        }

        Update();
    }

    private void Update() {
        yourhplabel.Text = $"HP: {your_hp}";
        enemyhplabel.Text = $"HP: {enemy_hp}";

        yourmanalabel.Text = $"{your_mana}/{your_max_mana}";
        enemymanalabel.Text = $"{enemy_mana}/{enemy_max_mana}";
    }



    public override void _Ready() {
        Instance = this;

        yourhplabel = GetTree().Root.GetNode<Label>("Main/UI/YourHP");
        enemyhplabel = GetTree().Root.GetNode<Label>("Main/UI/EnemyHP");
        yourmanalabel = GetTree().Root.GetNode<Label>("Main/UI/YourMana");
        enemymanalabel = GetTree().Root.GetNode<Label>("Main/UI/EnemyMana");
    }
}