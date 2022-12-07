using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StegProg
{
    public class LSB : ISteganograph
    {
        public LSB() { }
        public Bitmap Hide(System.Drawing.Image container, BitArray secret)
        {
            Bitmap bitmap = Converter.convertImageToArgb(container);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr intPtr = bitmapData.Scan0;

            byte[] bytebuffer = new byte[bitmapData.Stride * bitmap.Height];

            if (bytebuffer.Length < secret.Length)
            {
                throw new Exception("Daten sind zu groß, um sie per LSB im Bild zu verstecken");
            }

            Marshal.Copy(intPtr, bytebuffer, 0, bytebuffer.Length);

            for (int i = 0; i < bytebuffer.Length; i++)
            {
                bool intendedvalueOfBit = false;

                if (i < secret.Length)
                {
                    intendedvalueOfBit = secret[i];
                }
                if (intendedvalueOfBit == true && bytebuffer[i] % 2 == 0)
                {
                    bytebuffer[i]++;
                }
                if (intendedvalueOfBit == false && bytebuffer[i] % 2 == 1)
                {
                    bytebuffer[i]--;
                }
            }

            Marshal.Copy(bytebuffer, 0, intPtr, bytebuffer.Length);

            bitmap.UnlockBits(bitmapData);

            bitmapData = null;
            bytebuffer = null;


            return bitmap;
        }

        public void Unvail(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Byte[] bytebuffer = new byte[bitmapData.Stride * bitmap.Height];

            IntPtr intPtr = bitmapData.Scan0;

            Marshal.Copy(intPtr, bytebuffer, 0, bytebuffer.Length);

            List<bool> tempResult = new List<bool>();

            for (int i = 0; i < bytebuffer.Length; i++)
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

            Console.WriteLine(Converter.convertBitsToString(result));
        }
    }
}
