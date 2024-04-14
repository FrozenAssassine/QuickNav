using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.ConverterCommand
{
    internal class ZipConverter : IConverter
    {
        public string[] InputTypes => new string[0];

        public string[] OutputTypes => new string[] { "zip" };

        public string[] ConvertTo(string[] paths, string OutputType)
        {
            string folder = Path.Combine(Path.GetTempPath(), "QuickNavConverter");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (string file in Directory.GetFiles(folder))
                File.Delete(file);

            string filename = Path.Combine(folder, Path.GetFileNameWithoutExtension(paths[0]) + ".zip");

            ZipArchive archive = ZipFile.Open(filename, ZipArchiveMode.Create);
            for (int i = 0; i < paths.Length; i++)
                archive.CreateEntryFromFile(paths[i], Path.GetFileName(paths[i]));
            archive.Dispose();

            return new string[] { filename };
        }
    }
}
