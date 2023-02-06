namespace ComputerConfiguratorApplication.Models
{
    using System;

    public class Memory : Component
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{ComponentType} - {Name} - {Type} - Part number: {PartNumber} - Price: {Price}";
        }
        
    }
}