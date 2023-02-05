namespace ComputerConfiguratorApplication.IO
{
    using System;
    using ComputerConfiguratorApplication.IO.Contracts;

    public class Reader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
