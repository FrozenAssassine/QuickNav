using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.ConverterCommand
{
    public static class ConverterRegistry
    {
        public static List<IConverter> Converters = new List<IConverter>()
        {
            new ImageConverter(),
            new ZipConverter(),
        };

        public static IConverter[] GetConverterFor(string[] extensions)
        {
            List<IConverter> converters = new List<IConverter>();
            for(int i = 0; i < Converters.Count; i++)
            {
                bool found = true;
                for(int j = 0; j < extensions.Length; j++)
                    if (!Converters[i].InputTypes.Contains(extensions[j]))
                        found = false;
                if(found || Converters[i].InputTypes.Length == 0)
                    converters.Add(Converters[i]);
            }
            return converters.ToArray();
        }

        public static bool InputExistsFor(string extension)
        {
            extension = extension.ToLower().Replace(".", "");
            for (int i = 0; i < Converters.Count; i++)
                if (Converters[i].InputTypes.Length == 0 || Converters[i].InputTypes.Contains(extension))
                    return true;
            return false;
        }
    }
}
