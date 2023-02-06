namespace ComputerConfiguratorApplication.Models
{
    public class CPU : Component
    {
        public string Socket { get; set; }

        public string SupportedMemory { get; set; }

        public override string ToString()
        {
            return $"{ComponentType} - {Name} - {Socket}, {SupportedMemory} - Part number: {PartNumber} - Price: {Price}";
        }
    }
}