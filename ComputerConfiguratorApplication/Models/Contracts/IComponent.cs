namespace ComputerConfiguratorApplication.Models.Contracts
{
    public interface IComponent
    {
        public string ComponentType { get; set; }

        public string PartNumber { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        string ComponentSelectedMessage();
    }
}
