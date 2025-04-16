public struct CardStats{
    public int Cost;
    public int Health;
    public int Damaged;
    public int Attack;
    public List<Passives> passives;

    public CardStats(CardData data){
        Cost = data.cost;
        Health = data.health;
        Attack = data.attack;
        passives = [..data.passives];
    }

    public CardStats(){
        passives = [];
    }
}