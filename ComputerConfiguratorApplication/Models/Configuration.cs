namespace ComputerConfiguratorApplication.Models
{
    using System.Text;

    /// <summary>
    /// This class represents component configuration
    /// </summary>
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

            if (CPU != null)
            {
                sb.AppendLine($"{CPU.ComponentType} - {CPU.Name} - {CPU.Socket} - {CPU.SupportedMemory}");
            }

            if (Memory != null)
            {
                sb.AppendLine($"{Motherboard.ComponentType} - {Motherboard.Name} - {Motherboard.Socket}");
            }

            if (Motherboard != null)
            {
                sb.AppendLine($"{Memory.ComponentType} - {Memory.Name} - {Memory.Type}");
            }
            
            sb.AppendLine($"Price: {TotalPrice}");

            return sb.ToString();
        }
    }
}
