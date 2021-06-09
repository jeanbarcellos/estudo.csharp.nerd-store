using System;
using System.Linq;

namespace NerdStore.Pagamentos.AntiCorruption
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(x => x[new Random().Next(x.Length)]).ToArray());
        }
    }
}
