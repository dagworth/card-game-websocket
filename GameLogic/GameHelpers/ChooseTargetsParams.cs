public struct ChooseTargetsParams(){
    public TargetTypes TargetType { get; set; }
    public List<CardStatus> TargetList { get; set; } = [];
    public int TargetCount { get; set; } = 1;
    public Tribes? TargetTribe { get; set; }
    public int TargetMaxCost { get; set; } = -1;
    public int TargetMinCost { get; set; } = -1;

    //public bool CountCanBeLess { get; set; } = false;

    public void Filter(){
        for(int i = 0; i < TargetList.Count; i++){
            if(TargetMaxCost != -1 && TargetList[i].cost > TargetMaxCost){
                TargetList.RemoveAt(i--);
                continue;
            }

            if(TargetMinCost != -1 && TargetList[i].cost < TargetMinCost){
                TargetList.RemoveAt(i--);
                continue;
            }

            if(TargetList[i].tribe != TargetTribe){
                TargetList.RemoveAt(i--);
                continue;
            }
        }
    }
}