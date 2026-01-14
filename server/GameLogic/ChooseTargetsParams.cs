namespace server.GameLogic;

using server.GameLogic.Entities;
using server.ServerLogic;

public class ChooseTargetsParams(int plr_id, List<TargetTypes> stuff) {
    public int plr_id = plr_id;
    public List<int> TargetList { get; set; } = [];
    public bool Mandatory { get; set; } = true;
    public int TargetCount { get; set; } = 1;
    public Tribes TargetTribe { get; set; } = Tribes.None;
    public int TargetMaxCost { get; set; } = -1;
    public int TargetMinCost { get; set; } = -1;

    public List<int> ValidCounts { get; set; } = [1]; //not implemented but not important

    public void Filter(int game_id) {
        List<CardEntity> list = GetList(game_id);
        List<int> temp_misc_list = [];

        if (stuff.Contains(TargetTypes.YourPlayer) || stuff.Contains(TargetTypes.Allies))
            temp_misc_list.Add(-1);

        if (stuff.Contains(TargetTypes.EnemyPlayer) || stuff.Contains(TargetTypes.Enemies))
            temp_misc_list.Add(-2);

        for (int i = 0; i < list.Count; i++) {
            if (TargetMaxCost != -1 && list[i].Stats.Cost > TargetMaxCost) {
                list.RemoveAt(i--);
                continue;
            }

            if (TargetMinCost != -1 && list[i].Stats.Cost < TargetMinCost) {
                list.RemoveAt(i--);
                continue;
            }

            if (TargetTribe != Tribes.None) {
                if (!list[i].Tribes.Contains(TargetTribe)) {
                    list.RemoveAt(i--);
                    continue;
                }
            }
        }

        TargetList = [.. list.Select(x => x.Id), .. temp_misc_list];
    }

    private List<CardEntity> GetList(int game_id) {
        GameEntity game = GameManager.GetGame(game_id);
        PlayerEntity plr = game.plrs.GetPlayer(plr_id);
        PlayerEntity other_plr = game.plrs.GetOtherPlayer(plr_id);

        List<CardEntity> list = [];
        foreach (TargetTypes t in stuff) {
            switch (t) {
                case TargetTypes.YourUnitsInHand:
                    foreach (CardEntity card in plr.Hand) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.EnemyUnitsInHand:
                    foreach (CardEntity card in other_plr.Hand) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.YourSpellsInHand:
                    foreach (CardEntity card in plr.Hand) {
                        if (card.Type == CardTypes.Spell || card.Type == CardTypes.FastSpell) list.Add(card);
                    }
                    break;
                case TargetTypes.EnemySpellsInHand:
                    foreach (CardEntity card in other_plr.Hand) {
                        if (card.Type == CardTypes.Spell || card.Type == CardTypes.FastSpell) list.Add(card);
                    }
                    break;
                case TargetTypes.YourUnitsInVoid:
                    foreach (CardEntity card in plr.Void) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.EnemyUnitsInVoid:
                    foreach (CardEntity card in other_plr.Void) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.YourSpellsInVoid:
                    foreach (CardEntity card in plr.Void) {
                        if (card.Type == CardTypes.Spell || card.Type == CardTypes.FastSpell) list.Add(card);
                    }
                    break;
                case TargetTypes.EnemySpellsInVoid:
                    foreach (CardEntity card in other_plr.Void) {
                        if (card.Type == CardTypes.Spell || card.Type == CardTypes.FastSpell) list.Add(card);
                    }
                    break;
                case TargetTypes.YourUnits:
                    foreach (CardEntity card in plr.Board) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.EnemyUnits:
                    foreach (CardEntity card in other_plr.Board) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.YourPlayer:

                    break;
                case TargetTypes.EnemyPlayer:

                    break;
                case TargetTypes.Enemies:
                    foreach (CardEntity card in other_plr.Board) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.Allies:
                    foreach (CardEntity card in plr.Board) {
                        if (card.Type == CardTypes.Unit) list.Add(card);
                    }
                    break;
                case TargetTypes.PlayedFastSpell:
                    //implement it later
                    break;
                case TargetTypes.PlayedSpell:
                    //implement it later
                    break;
            }
        }
        return list;
    }
}