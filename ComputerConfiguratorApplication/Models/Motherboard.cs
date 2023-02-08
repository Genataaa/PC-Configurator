namespace ComputerConfiguratorApplication.Models
{
    /// <summary>
    /// This class represents Motherboard component
    /// </summary>
    public class Motherboard : Component
    {
        public string Socket { get; set; }

        public override string ToString()
        {
            return $"{ComponentType} - {Name} - {Socket} - Part number: {PartNumber} - Price: {Price}";
        }
    }
}
