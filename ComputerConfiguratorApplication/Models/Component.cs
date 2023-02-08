namespace ComputerConfiguratorApplication.Models
{
    /// <summary>
    /// This is abstract class that represents base component entity.
    /// </summary>
    public abstract class Component
    {
        public string ComponentType { get; set; }

        public string PartNumber { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ComponentSelectedMessage()
        {
            return $"Successfully selected {ComponentType} with part number {PartNumber}";
        }
    }
}
