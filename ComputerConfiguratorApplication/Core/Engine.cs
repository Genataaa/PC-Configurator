namespace ComputerConfiguratorApplication.Core
{
    using ComputerConfiguratorApplication.Core.Contracts;
    using ComputerConfiguratorApplication.IO.Contracts;
    using ComputerConfiguratorApplication.IO;
    using ComputerConfiguratorApplication.Data.Models;

    public class Engine : IEngine
    {
        private IWriter writer;
        private IReader reader;
        private Inventory inventory;

        public Engine(Inventory repository)
        {
            writer = new Writer();
            reader = new Reader();
            this.inventory = repository;
        }

        public void Run()
        {
            writer.Write("Engine Working");
        }
    }
}
