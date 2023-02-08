namespace ComputerConfiguratorApplication.Models
{
    using System;

    /// <summary>
    /// This class represents Memory component
    /// </summary>
    public class Memory : Component
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{ComponentType} - {Name} - {Type} - Part number: {PartNumber} - Price: {Price}";
        }
        
    }
}