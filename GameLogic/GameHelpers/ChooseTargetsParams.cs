public struct ChooseTargetsParams(List<CardStatus> list){
    public TargetTypes TargetType { get; set; }
    public List<int> TargetList { get; set; } = [..list.Select(x => x.card_id)];
    public bool Mandatory { get; set; } = true;
    public int TargetCount { get; set; } = 1;
    public Tribes? TargetTribe { get; set; }
    public int TargetMaxCost { get; set; } = -1;
    public int TargetMinCost { get; set; } = -1;

    //public bool CountCanBeLess { get; set; } = false;

    public void Filter(){
        for(int i = 0; i < list.Count; i++){
            if(TargetMaxCost != -1 && list[i].cost > TargetMaxCost){
                list.RemoveAt(i--);
                continue;
            }

            if(TargetMinCost != -1 && list[i].cost < TargetMinCost){
                list.RemoveAt(i--);
                continue;
            }

            bool found_tribe = false;
            foreach(Tribes tribe in list[i].tribes){
                if(tribe == TargetTribe){
                    found_tribe = true;
                    break;
                }
            }

            if(!found_tribe){
                list.RemoveAt(i--);
                continue;
            }
        }

        TargetList = [..list.Select(x => x.card_id)];
    }
}