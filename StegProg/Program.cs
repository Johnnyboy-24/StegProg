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
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public class Program
    {
        
        
        static void Main(string[] args)
        {
            //string[] parameters = Console.ReadLine().Split(' ');
            string path = @"C:\Users\b190\Pictures\M113086-Neapolitanische_Pizza_112317_Titelbild-Q75-750.jpg";
            string fileName = @"C:\Users\b190\Pictures\neuesBild.bmp";
            string secret = "This is a secret";


            while ( true)
            {
                Bitmap bitmap = GetBitmap(path);
                bitmap = getArgb(bitmap);
                Bitmap steg = hideLSB(bitmap, getBits(secret));
                showLSB(steg, getBits(secret));
                //saveImage(bitmap, fileName);
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
        static string convertBitsToString(BitArray bitArray)
        {
            if (bitArray.Length % 8 != 0)
            {
                throw new Exception("incomplete input");
            }
            Byte[] bytes= new Byte[bitArray.Length/8];
            bitArray.CopyTo(bytes, 0);
            return Encoding.Unicode.GetString(bytes);   

        }

        static BitArray getBits(string secret)
        {
            return new BitArray( Encoding.Unicode.GetBytes(secret));

        }
        
        static Bitmap hideLSB(Image image, BitArray data)
        {
            Bitmap bitmap = getArgb(image);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr intPtr= bitmapData.Scan0;

            byte[] bytebuffer = new byte[bitmapData.Stride * bitmap.Height];

            if (bytebuffer.Length < data.Length)
            {
                throw new Exception("Daten sind zu groß, um sie per LSB im Bild zu verstecken");
            }

            Marshal.Copy(intPtr, bytebuffer, 0, bytebuffer.Length);

            for(int i = 0; i < data.Length; i ++ )
            {
                if (data[i] == true && bytebuffer[i] % 2 == 0)
                {
                    bytebuffer[i] ++;
                }
                if (data[i] == false && bytebuffer[i] % 2 == 1)
                {
                    bytebuffer[i] --;
                }
            }

            Marshal.Copy(bytebuffer, 0, intPtr, bytebuffer.Length);

            bitmap.UnlockBits(bitmapData);

            bitmapData = null;
            bytebuffer = null;


            return bitmap;
        }

        static void showLSB(Bitmap bitmap, BitArray bitarray)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Byte[] bytebuffer = new byte[bitmapData.Stride * bitmap.Height];

            IntPtr intPtr = bitmapData.Scan0;

            Marshal.Copy(intPtr, bytebuffer, 0, bytebuffer.Length);

            List<bool> tempResult = new List<bool>();

            for ( int i = 0; i < bitarray.Length; i ++)
            {
                if (bytebuffer[i] % 2 == 0)
                {
                    tempResult.Add(false);
                }
                else
                {
                    tempResult.Add(true);
                }
            }
            BitArray result = new BitArray(tempResult.ToArray());

            Console.WriteLine(result.ToString());
            Console.WriteLine(convertBitsToString(result));
            Console.WriteLine(result.Equals(bitarray));
            Console.ReadLine();
        }

        static void saveImage(Bitmap bitmap, string filename)
        {
            bitmap.Save(filename, ImageFormat.Bmp);
        }
    }
}
