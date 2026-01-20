using System.IO;
using System.Reflection;
using System.Collections.Generic;
using shared.DTOs;
using CsvHelper;
using System.Globalization;
using Godot;

public partial class DataLoader : Node {
    private static readonly Dictionary<string, CardDataDTO> dataMap = [];

    public static CardDataDTO GetData(string name) {
        if (dataMap.ContainsKey(name)) return dataMap[name];
        GD.Print($"there is no data logic for {name}");
        return new CardDataDTO();
    }

    public override void _Ready() {
        var assembly = Assembly.GetExecutingAssembly();

        Stream stream = assembly.GetManifestResourceStream("client.shared.card_data.csv")!;
        StreamReader reader = new(stream);
        CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<CardDataDTO>();
        foreach (var record in records) {
            string name = csv.GetField("Name") ?? "";
            dataMap[name] = record;
        }
    }
}