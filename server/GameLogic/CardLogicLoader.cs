using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

public static class CardLogicLoader {
    private static readonly Dictionary<string, CardEffect> logicMap = new();
    private static readonly CardEffect null_effect = new C_Nothing();

    static CardLogicLoader() {
        var classes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CardEffect)));

        foreach (var card in classes) {
            var instance = (CardEffect)Activator.CreateInstance(card)!;
            string key = card.Name[2..].Replace("_", " ");
            logicMap[key] = instance;
        }
    }

    public static CardEffect GetLogic(string name) {
        if (logicMap.ContainsKey(name)) {
            return logicMap[name];
        }
        Console.WriteLine($"there is no class logic for {name}");
        return null_effect;
    }

    public static bool HasEffect(string card_name, string method) {
        var result = GetLogic(card_name).GetType().GetMethod(method);

        if (result == null) return false;

        return result.DeclaringType != typeof(CardEffect);
    }
}

public class C_Nothing : CardEffect { }