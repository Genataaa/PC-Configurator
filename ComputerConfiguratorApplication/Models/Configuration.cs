namespace ComputerConfiguratorApplication.Models
{
    using System.Text;

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

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{CPU.ComponentType} - {CPU.Name} - {CPU.Socket} - {CPU.SupportedMemory}");
            sb.AppendLine($"{Motherboard.ComponentType} - {Motherboard.Name} - {Motherboard.Socket}");
            sb.AppendLine($"{Memory.ComponentType} - {Memory.Name} - {Memory.Type}");
            sb.AppendLine($"Price: {TotalPrice}");

            return sb.ToString();
        }
    }
}
