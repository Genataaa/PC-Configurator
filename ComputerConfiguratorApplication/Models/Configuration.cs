namespace ComputerConfiguratorApplication.Models
{

    public class Configuration
    {
        public CPU CPU { get; set; }

        public Motherboard Motherboard { get; set; }

        public Memory Memory { get; set; }

        public decimal TotalPrice 
            => 
            (CPU != null ? CPU.Price : 0) +
            (Motherboard != null ? Motherboard.Price : 0) +
            (Memory != null ? Memory.Price : 0);
    }
}
