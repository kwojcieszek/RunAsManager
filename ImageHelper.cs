using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace RunAsManager
{
    internal static class ImageHelper
    {
        public static BitmapImage ToBitmapImage(Byte[] value)
        {
            if (value != null && value is not null)
            {
                byte[] ByteArray = value as byte[];
                BitmapImage bmp = new();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(ByteArray);
                bmp.EndInit();
                return bmp;
            }

            return null;
        }

        public static byte[] ImageToByte(Image img)
        {
            if (img == null)
                return null;

            using var stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

        public static Bitmap IconFromFilePath(string filePath)
        {
            try
            {
                return Icon.ExtractAssociatedIcon(filePath).ToBitmap();
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }

            return null;
        }
    }
}