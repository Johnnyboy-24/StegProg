using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    internal class BitMapService
    {
        public BitMapService() { }

        public Bitmap getBitmap(string path)
        {
            return toBitmap(readImage(path));
        }
        private Image readImage(string path)
        {
            Image image = Image.FromFile(path);
            return image;
        }
        private Bitmap toBitmap(Image image)
        {
            return new Bitmap(image);
        }
    }
}
