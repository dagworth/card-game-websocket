using System.Reflection;
using shared.DTOs;
using CsvHelper;
using System.Globalization;

public static class DataLogicLoader {
    private static readonly Dictionary<string, CardEffect> logicMap = [];
    private static readonly Dictionary<string, CardDataDTO> dataMap = [];

    static DataLogicLoader() {
        var assembly = Assembly.GetExecutingAssembly();

        var classes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CardEffect)));

        foreach (var card in classes) {
            var instance = (CardEffect)Activator.CreateInstance(card)!;
            string key = card.Name[2..].Replace("_", " ");
            logicMap[key] = instance;
        }

        Stream stream = assembly.GetManifestResourceStream("server.shared.card_data.csv")!;
        StreamReader reader = new(stream);
        CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<CardDataDTO>();
        foreach (var record in records) {
            string name = csv.GetField("Name") ?? "";
            if(name.Equals("")) Console.WriteLine("u f'd up (forgot to name a dude in card_data)");
            dataMap[name] = record;
        }
    }

    public static CardEffect GetLogic(string name) {
        if (logicMap.TryGetValue(name, out CardEffect? value)) {
            return value;
        }
        Console.WriteLine($"there is no class logic for {name}");
        return new C_Nothing();
    }

    public static CardDataDTO GetData(string name) {
        if (dataMap.TryGetValue(name, out CardDataDTO? value)) {
            return value;
        }
        Console.WriteLine($"there is no data logic for {name}");
        return new CardDataDTO();
    }

    public static bool HasEffect(string card_name, string method) {
        var result = GetLogic(card_name).GetType().GetMethod(method);

        if (result == null) return false;

        return result.DeclaringType != typeof(CardEffect);
    }
}

public class C_Nothing : CardEffect { }