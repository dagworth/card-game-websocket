using Fleck;

public static class GameManager {
    private static int counter = 0;
    private static readonly Dictionary<int,GameEntity> games = [];
    private static readonly Dictionary<int,PlayerEntity> plrs = [];

    public static GameEntity CreateGame(int plr0_id, int plr1_id){
        int game_id = counter++;

        List<string> deck0 = ["Haunting Scream","Dave","Gary","Bob","Carol","Alice","Bob","Carol","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice"];
        List<string> deck1 = ["Freddy","Gary","Carol","Bob","Carol","Alice","Bob","Carol","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob"];

        GameEntity clone = new GameEntity(game_id, plr0_id, plr1_id, deck0, deck1);
        games[game_id] = clone;
        return clone;
    }

    public static GameEntity GetGame(int game_id){
        return games[game_id];
    }

    public static PlayerEntity GetPlayer(int plr_id){
        return plrs[plr_id];
    }

    public static PlayerEntity AddPlayer(int plr_id, PlayerEntity plr){
        return plrs[plr_id] = plr;
    }
}