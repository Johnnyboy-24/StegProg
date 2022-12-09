    using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public static class Utility
    {
        public static Bitmap GenerateBitmap(string path)
        {
            return Converter.convertImageToArgb( (Bitmap)Image.FromFile(path));
        }

        public static Bitmap GetBitmap(string path)
        {
            return new Bitmap(path);
        }

        public static BitArray getBits(string secret)
        {
            return new BitArray(Encoding.Unicode.GetBytes(secret));

        }

        public static void saveImage(Bitmap bitmap, string filename)
        {
            bitmap.Save(filename, ImageFormat.Bmp );
        }
    }
}
