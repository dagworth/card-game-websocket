public class ChooseTargetsParams(List<CardEntity> list){
    public TargetTypes TargetType { get; set; }
    public List<int> TargetList { get; set; } = [..list.Select(x => x.Id)];
    public bool Mandatory { get; set; } = true;
    public int TargetCount { get; set; } = 1;
    public Tribes TargetTribe { get; set; } = Tribes.None;
    public int TargetMaxCost { get; set; } = -1;
    public int TargetMinCost { get; set; } = -1;

    public bool CountCanBeLess { get; set; } = false;

    public void Filter(){
        for(int i = 0; i < list.Count; i++){
            if(TargetMaxCost != -1 && list[i].Stats.Cost > TargetMaxCost){
                list.RemoveAt(i--);
                continue;
            }

            if(TargetMinCost != -1 && list[i].Stats.Cost < TargetMinCost){
                list.RemoveAt(i--);
                continue;
            }

            if(TargetTribe != Tribes.None){
                if(!list[i].Tribes.Contains(TargetTribe)){
                    list.RemoveAt(i--);
                    continue;
                }
            }
        }

        TargetList = [..list.Select(x => x.Id)];
    }
}