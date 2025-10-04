public class Buff {
    public CardEntity? card;
    public int Cost;
    public int Cost_Fixed;
    public int Attack;
    public int Attack_Fixed;
    public int Health;
    public int Health_Fixed;
    public List<Passives> passives = [];

    public void RemoveBuff(){
        if(card == null){
            Console.WriteLine("you fucked up (buff was attached to nobody)");
        }
        card!.RemoveTempBuff(this);
    }
}