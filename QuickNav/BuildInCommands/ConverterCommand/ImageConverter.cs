using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace QuickNav.BuildInCommands.ConverterCommand
{
    public class ImageConverter : IConverter
    {
        public string[] InputTypes => new string[] { "bmp", "png", "jpg", "jpeg", "ico" };

        public string[] OutputTypes => new string[] { "bmp", "png", "jpg", "jpeg", "gif", "ico" };

        public string[] ConvertTo(string[] paths, string OutputType)
        {
            string folder = Path.Combine(Path.GetTempPath(), "QuickNavConverter");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (string file in Directory.GetFiles(folder))
                File.Delete(file);

            if (OutputType == "ico")
            {
                List<string> newPaths = new List<string>();
                for (int i = 0; i < paths.Length; i++)
                {
                    string p = Path.Combine(folder, Path.GetFileNameWithoutExtension(paths[i]) + "." + OutputType);
                    if (Path.GetExtension(paths[i]).ToLower() == ".ico")
                    {
                        File.Copy(paths[i], p);
                    }
                    else
                    {
                        File.WriteAllBytes(p, ConvertHelper.BitmapToIcon(new Bitmap(paths[i])));
                    }
                    newPaths.Add(p);
                }

                return newPaths.ToArray();
            }
            else
            {
                ImageFormat format = ImageFormat.Png;
                switch (OutputType)
                {
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                        format = ImageFormat.Jpeg;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                }

                List<string> newPaths = new List<string>();
                for(int i = 0; i < paths.Length; i++)
                {
                    Bitmap bmp;
                    if (Path.GetExtension(paths[i]).ToLower() == ".ico")
                        bmp = new Icon(paths[i]).ToBitmap();
                    else
                        bmp = new Bitmap(paths[i]);
                    string p = Path.Combine(folder, Path.GetFileNameWithoutExtension(paths[i]) + "." + OutputType);
                    bmp.Save(p, format);
                    newPaths.Add(p);
                }

                return newPaths.ToArray();
            }
        }
    }
}
