using Fleck;

public static class GameManager {
    private static int counter = 0;
    private static readonly Dictionary<int,Game> games = [];
    private static readonly Dictionary<int,int> plr_to_game_id = [];

    public static Game CreateGame(int plr1_id, int plr2_id){
        int game_id = counter++;

        List<string> deck1 = ["Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice"];
        List<string> deck2 = ["Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob","Bob","Alice","Bob"];

        Game clone = new Game(game_id, plr1_id, plr2_id, deck1, deck2);
        games[game_id] = clone;

        plr_to_game_id[plr1_id] = game_id;
        plr_to_game_id[plr2_id] = game_id;

        return clone;
    }

    public static Game GetGameById(int game_id){
        return games[game_id];
    }

    public static Game GetGame(int plr_id){
        return games[plr_to_game_id[plr_id]];
    }
}