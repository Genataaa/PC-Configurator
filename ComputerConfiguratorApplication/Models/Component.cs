namespace ComputerConfiguratorApplication.Models
{
    public abstract class Component
    {
        public string ComponentType { get; set; }

        public string PartNumber { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
