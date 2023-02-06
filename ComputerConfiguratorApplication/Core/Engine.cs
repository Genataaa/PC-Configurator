namespace ComputerConfiguratorApplication.Core
{
    using System.Collections.Generic;

    using ComputerConfiguratorApplication.Core.Contracts;
    using ComputerConfiguratorApplication.IO.Contracts;
    using ComputerConfiguratorApplication.IO;
    using ComputerConfiguratorApplication.Data.Models;
    using ComputerConfiguratorApplication.Models;
    using System.Linq;

    public class Engine : IEngine
    {
        private IWriter writer;
        private IReader reader;
        private Inventory inventory;
        private Configuration configuration;
        private HashSet<Configuration> possibleConfigurations;

        public Engine(Inventory repository)
        {
            writer = new Writer();
            reader = new Reader();
            this.inventory = repository;
            configuration = new Configuration();
            possibleConfigurations = new HashSet<Configuration>();
        }

        public void Run()
        {
            PrintInventoryComponents();

            writer.WriteLine($"Please enter part number(s):");

            var index = 1;

            while (true)
            {
                var partNumber = string.Empty;

                if (index == 1)
                {
                    writer.WriteLine($"CPU part number:");
                    partNumber = reader.ReadLine();

                    var CPU = inventory.CPUs.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (CPU != null)
                    {
                        configuration.CPU = CPU;
                        writer.WriteLine($"Successfully selected {CPU.ComponentType} with part number {CPU.PartNumber}");
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        continue;
                    }
                }
                else if (index == 2)
                {
                    writer.WriteLine($"Motherboard part number:");
                    partNumber = reader.ReadLine();

                    var motherboard = inventory.Motherboards.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (motherboard != null)
                    {
                        configuration.Motherboard = motherboard;
                        writer.WriteLine($"Successfully selected {motherboard.ComponentType} with part number {motherboard.PartNumber}");
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        continue;
                    }
                }
                else if (index == 3)
                {
                    writer.WriteLine($"Memory part number:");
                    partNumber = reader.ReadLine();

                    var memory = inventory.Memory.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (memory != null)
                    {
                        configuration.Memory = memory;
                        writer.WriteLine($"Successfully selected {memory.ComponentType} with part number {memory.PartNumber}");
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        continue;
                    }
                }

                PrintCurrentConfiguration(configuration);

                var action = ActionOptionsMenu();

                if (action == "2")
                {
                    //todo
                    break;
                }
                else if (action == "0")
                {
                    index = 1;
                    ClearConfiguration();
                    continue;
                }

                index++;

                if (index == 4)
                {
                    break;
                }
            }

            var isConfigurationValid = IsComponentsCompatible(configuration);

            if (index == 4)
            {

                writer.WriteLine($"Total: {configuration.TotalPrice}");
            }
            else
            {
                FindAllPossibleConfigurations();

                writer.WriteLine($"There are {possibleConfigurations.Count} possible combinations:");
                writer.WriteLine("");

                for (int i = 1; i <= possibleConfigurations.Count; i++)
                {
                    writer.WriteLine($"{i}");
                    writer.WriteLine(configuration.ToString());
                }
            }
        }

        private void FindAllPossibleConfigurations()
        {
            var CPU = configuration.CPU;

            if (configuration.Motherboard == null)
            {
                possibleConfigurations = inventory
                    .Motherboards
                    .Where(m => m.Socket == CPU.Socket)
                    .Select(motherboard => new Configuration()
                    {
                        CPU = CPU,
                        Motherboard = motherboard,
                    })
                    .ToHashSet();
            }
            else
            {
                var currentConfiguration = new Configuration()
                {
                    CPU = configuration.CPU,
                    Motherboard = configuration.Motherboard,
                };

                possibleConfigurations.Add(configuration);
            }

            var compatibleMemoryCollection = inventory
                .Memory
                .Where(m => m.Type == CPU.SupportedMemory)
                .ToHashSet();

            foreach (var currentConfiguration in possibleConfigurations)
            {
                foreach (var memory in compatibleMemoryCollection)
                {
                    currentConfiguration.Memory = memory;
                }
            }
        }

        private bool IsComponentsCompatible(Configuration configuration)
        {
            var CPU = configuration.CPU;

            var compatibleMotherboards = inventory
                .Motherboards
                .Where(m => m.Socket == CPU.Socket)
                .ToHashSet();

            if (!compatibleMotherboards.Contains(configuration.Motherboard))
            {
                return false;
            }

            var compatibleMemoryCollection = inventory
                .Memory
                .Where(m => m.Type == CPU.SupportedMemory)
                .ToHashSet();

            if (!compatibleMemoryCollection.Contains(configuration.Memory))
            {
                return false;
            }

            return true;
        }

        private string ActionOptionsMenu()
        {
            writer.WriteLine("Please select action to continue:");
            writer.WriteLine("");

            writer.WriteLine("1 - Continue");
            writer.WriteLine("2 - Show all possible configurations");
            writer.WriteLine("0 - Go back to the begining");

            return reader.ReadLine();
        }

        private void PrintInventoryComponents()
        {
            writer.WriteLine("");
            writer.WriteLine("CPUs:");
            VisualizeComponents(inventory.CPUs);

            writer.WriteLine("");
            writer.WriteLine("Motherboards:");
            VisualizeComponents(inventory.Motherboards);

            writer.WriteLine("");
            writer.WriteLine("Memory:");
            VisualizeComponents(inventory.Memory);

            writer.WriteLine("");
        }

        private void PrintCurrentConfiguration(Configuration configuration)
        {
            writer.WriteLine("Current Configuration:");

            if (configuration.CPU != null)
            {
                writer.WriteLine("CPU:");
                writer.WriteLine(configuration.CPU.ToString());
            }

            if (configuration.Motherboard != null)
            {
                writer.WriteLine("Motherboard:");
                writer.WriteLine(configuration.Motherboard.ToString());
            }

            if (configuration.Memory != null)
            {
                writer.WriteLine("Memory");
                writer.WriteLine(configuration.Memory.ToString());
            }
        }

        private void ClearConfiguration()
        {
            configuration.CPU = null;
            configuration.Motherboard = null;
            configuration.Memory = null;
        }

        private void VisualizeComponents<T>(IEnumerable<T> componentsList)
        {
            foreach (T component in componentsList)
            {
                writer.WriteLine(component.ToString());
            }
        }
    }
}
