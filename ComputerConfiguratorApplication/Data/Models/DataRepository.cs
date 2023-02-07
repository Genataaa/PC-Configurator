using ComputerConfiguratorApplication.Models;

namespace ComputerConfiguratorApplication.Data.Models
{
    public class DataRepository
    {
        public HashSet<CPU> CPUs { get; set; } = new HashSet<CPU>();

        public HashSet<Memory> Memory { get; set; } = new HashSet<Memory>();

        public HashSet<Motherboard> Motherboards { get; set; } = new HashSet<Motherboard>();
    }
}
