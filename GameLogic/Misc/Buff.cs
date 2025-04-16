public class Buff {
    public CardEntity? card;
    public int Cost;
    public int Attack;
    public int Health;
    public List<Passives> passives = [];

    public void RemoveBuff(){
        if(card == null){
            Console.WriteLine("you fucked up (buff was attached to nobody)");
        }
        card!.RemoveTempBuff(this);
    }
}