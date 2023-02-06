namespace ComputerConfiguratorApplication.Models
{
    public class Motherboard : Component
    {
        public string Socket { get; set; }

        public override string ToString()
        {
            return $"{ComponentType} - {Name} - {Socket} - Part number: {PartNumber} - Price: {Price}";
        }
    }
}
