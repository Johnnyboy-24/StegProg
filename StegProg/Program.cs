using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public class Program
    {
        
        
        static void Main(string[] args)
        {
            string fileName = @"C:\Users\b190\Pictures\neuesBildd.bmp";
            string secret = "This is a secret";
            string[] parameters;
            Parser parser = new Parser();
            LSB lSB = new LSB();

            while (!parser.exit)
            {
                parameters = Console.ReadLine().Split(' ');
                parser.parse(parameters);
                Bitmap imageWithSecret = lSB.Hide(GetBitmap(parser.path), getBits(secret));
                lSB.Unvail(imageWithSecret);
                Console.ReadKey();
            }
        }
        
        static Bitmap GetBitmap(string path)
        {
            return new Bitmap(Image.FromFile(path));
        }
        
        static Bitmap getArgb(Image image)
        {
            Bitmap bitmapargb = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            using(Graphics graphics = Graphics.FromImage(bitmapargb))
            {
                graphics.DrawImage(image, new Rectangle(0,0, bitmapargb.Width, bitmapargb.Height));
                graphics.Flush();
            }
            return bitmapargb;
        }

        static BitArray getBits(string secret)
        {
            return new BitArray( Encoding.Unicode.GetBytes(secret));

        }
        
        static void saveImage(Bitmap bitmap, string filename)
        {
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        
    }
}
