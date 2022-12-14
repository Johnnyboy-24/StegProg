using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public static class Converter
    {
        public static Bitmap convertImageToArgb(Image image)
        {
            Bitmap bitmapargb = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bitmapargb))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, bitmapargb.Width, bitmapargb.Height));
                graphics.Flush();
            }
            return bitmapargb;
        }

        public static string convertBitsToString(BitArray bitArray)
        {
            Byte[] bytes = new Byte[bitArray.Length / 8 + 1 ];
            bitArray.CopyTo(bytes, 0);
            string message = Encoding.Unicode.GetString(bytes);
            return message;

        }
    }
}
