using ComputerConfiguratorApplication.Core;
using ComputerConfiguratorApplication.Data.Models;
using System.Text.Json;

string filePath = @"..\..\..\Data\pc-store-inventory.json";

var inventory = JsonSerializer.Deserialize<Inventory>
            (File.ReadAllText(filePath));

var engine = new Engine(inventory);
engine.Run();