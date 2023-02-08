namespace ComputerConfiguratorApplication.Core
{
    using System.Collections.Generic;

    using ComputerConfiguratorApplication.Core.Contracts;
    using ComputerConfiguratorApplication.IO.Contracts;
    using ComputerConfiguratorApplication.IO;
    using ComputerConfiguratorApplication.Data.Models;
    using ComputerConfiguratorApplication.Models;
    using System.Linq;

    /// <summary>
    ///Engine class
    /// </summary>
    public class Engine : IEngine
    {
        private IWriter writer;
        private IReader reader;
        private DataRepository inventory;
        private Configuration configuration;
        private HashSet<Configuration> possibleConfigurations;

        public Engine(DataRepository repository)
        {
            writer = new Writer();
            reader = new Reader();
            this.inventory = repository;
            configuration = new Configuration();
            possibleConfigurations = new HashSet<Configuration>();
        }

        /// <summary>
        /// This method starts program
        /// </summary>
        public void Run()
        {
            var index = MainMenu();

            var isConfigurationValid = IsComponentsCompatible(configuration);

            if (!isConfigurationValid)
            {
                MainMenu();
            }

            if (index == 4)
            {
                writer.WriteLine($"Configuration:\n{configuration}");
            }
            else
            {
                FindAllPossibleConfigurations();

                writer.WriteLine($"There are {possibleConfigurations.Count} possible combinations:");
                writer.WriteLine("");

                var i = 1;
                foreach (var currentConfiguration in possibleConfigurations)
                {
                    writer.WriteLine($"{i++}");
                    writer.WriteLine(currentConfiguration.ToString());
                }
            }
        }

        /// <summary>
        /// This method prints main menu opptions in the console.
        /// </summary>
        /// <returns></returns>
        private int MainMenu()
        {
            var index = 1;
            while (true)
            {
                PrintInventoryComponents();
                writer.WriteLine($"Please enter part number:");

                var partNumber = string.Empty;

                if (index == 1)
                {
                    writer.WriteLine($"CPU part number:");
                    partNumber = reader.ReadLine();

                    var CPU = inventory.CPUs.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (CPU != null)
                    {
                        configuration.CPU = CPU;
                        writer.WriteLine(CPU.ComponentSelectedMessage());
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        writer.WriteLine("");
                        continue;
                    }
                }
                else if (index == 2)
                {
                    writer.WriteLine($"Memory part number:");

                    partNumber = reader.ReadLine();

                    var memory = inventory.Memory.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (memory != null)
                    {
                        configuration.Memory = memory;
                        writer.WriteLine(memory.ComponentSelectedMessage());
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        writer.WriteLine("");
                        continue;
                    }
                }
                else if (index == 3)
                {
                    writer.WriteLine($"Motherboard part number:");

                    partNumber = reader.ReadLine();

                    var motherboard = inventory.Motherboards.FirstOrDefault(c => c.PartNumber == partNumber);

                    if (motherboard != null)
                    {
                        configuration.Motherboard = motherboard;
                        writer.WriteLine(motherboard.ComponentSelectedMessage());
                    }
                    else
                    {
                        writer.WriteLine("ERROR: Invalid part number.");
                        writer.WriteLine("");
                        continue;
                    }
                }

                PrintCurrentConfiguration(configuration);

                var action = ActionOptionsMenu();

                if (action == "2")
                {
                    return index;
                }
                else if (action == "0")
                {
                    index = 1;
                    ClearConfiguration();
                    continue;
                }

                index++;

                //If index == 4 that mean user is selected all components of configuration.
                if (index == 4)
                {
                    return index;
                }
            }
        }

        /// <summary>
        /// This method search for all possible configurations, that contains components coosed by user.
        /// </summary>
        private void FindAllPossibleConfigurations()
        {
            var currentConfigurations = new List<Configuration>();

            var CPU = configuration.CPU;

            if (configuration.Memory == null)
            {
                currentConfigurations = inventory
                    .Memory
                    .Where(m => m.Type == CPU.SupportedMemory)
                    .Select(memory => new Configuration()
                    {
                        CPU = CPU,
                        Memory = memory,
                    })
                    .ToList();
            }
            else
            {
                var currentConfiguration = new Configuration()
                {
                    CPU = configuration.CPU,
                    Memory = configuration.Memory,
                };

                currentConfigurations.Add(configuration);
            }

            var compatibleMotherboardsCollection = inventory
                .Motherboards
                .Where(m => m.Socket == CPU.Socket)
                .ToHashSet();

            foreach (var currentConfiguration in currentConfigurations)
            {
                foreach (var motherboard in compatibleMotherboardsCollection)
                {
                    var configuration = new Configuration()
                    {
                        CPU = currentConfiguration.CPU,
                        Memory = currentConfiguration.Memory,
                        Motherboard = motherboard,
                    };

                    possibleConfigurations.Add(configuration);
                }
            }
        }

        /// <summary>
        /// This methood checks if components choosed by user are compatible.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private bool IsComponentsCompatible(Configuration configuration)
        {
            var CPU = configuration.CPU;

            var compatibleMemoryCollection = inventory
                .Memory
                .Where(m => m.Type == CPU.SupportedMemory)
                .ToHashSet();

            if (configuration.Memory != null && !compatibleMemoryCollection.Contains(configuration.Memory))
            {
                writer.WriteLine($"Memory of type {configuration.Memory.Type} is not compatible with the CPU");
                return false;
            }

            var compatibleMotherboards = inventory
                .Motherboards
                .Where(m => m.Socket == CPU.Socket)
                .ToHashSet();

            if (configuration.Motherboard != null && !compatibleMotherboards.Contains(configuration.Motherboard))
            {
                writer.WriteLine($"Motherboard of type {configuration.Motherboard.Socket} is not compatible with the CPU");
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method prints actions menu in the console.
        /// </summary>
        /// <returns></returns>
        private string ActionOptionsMenu()
        {
            writer.WriteLine("");
            writer.WriteLine("Please select action to continue:");
            writer.WriteLine("");

            writer.WriteLine("[1] - Continue");
            writer.WriteLine("[2] - Show all possible configurations");
            writer.WriteLine("[0] - Reset configuration");

            return reader.ReadLine();
        }

        /// <summary>
        /// This method prints all components in the inventory.
        /// </summary>
        private void PrintInventoryComponents()
        {
            writer.WriteLine("All available Components:");
            writer.WriteLine("");
            writer.WriteLine("CPUs:");
            VisualizeComponents(inventory.CPUs);

            writer.WriteLine("");
            writer.WriteLine("Memory:");
            VisualizeComponents(inventory.Memory);

            writer.WriteLine("");
            writer.WriteLine("Motherboards:");
            VisualizeComponents(inventory.Motherboards);

            writer.WriteLine("");
        }

        /// <summary>
        /// This method prints current configuration, choosed by user.
        /// </summary>
        /// <param name="configuration"></param>
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

        /// <summary>
        /// This method clear current configuration, choosed by user, and clear possibleConfigurations collection.
        /// </summary>
        private void ClearConfiguration()
        {
            configuration.CPU = null;
            configuration.Motherboard = null;
            configuration.Memory = null;
        }

        /// <summary>
        /// This method prints all components from collection that receive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentsList"></param>
        private void VisualizeComponents<T>(IEnumerable<T> componentsList)
        {
            foreach (T component in componentsList)
            {
                writer.WriteLine(component.ToString());
            }
        }
    }
}
