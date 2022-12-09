using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Bitmap Hide(Bitmap image, BitArray secret)
        {
            Color actual;
            Color newColor;
            int indexer = 0;
            bool lsbValue = false;
            byte alphaValue;
            Bitmap newImage = new Bitmap(image.Width, image.Height); 

            for (int i =0; i< image.Width; i++)
            {
                for (int j= 0; j<image.Height; j++)
                {
                    actual = image.GetPixel(i, j);
                    if(indexer < secret.Length)
                    {
                        lsbValue = secret[indexer];
                    }
                    if(lsbValue == true)
                    {
                        alphaValue = (byte)(actual.R | 1);
                    }
                    else
                    {
                        alphaValue = (byte)((actual.R & 254));                        
                    }
                    newColor = Color.FromArgb(actual.A, alphaValue, actual.G,actual.B);
                    newImage.SetPixel(i, j, newColor);

                    indexer++;
                }
            }
            return newImage;
        }

        public void Unvail(Bitmap image)
        {
            

            List<bool> tempResult = new List<bool>();
            Color color;

            for(int i = 0; i< image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    color = image.GetPixel(i,j);
                    tempResult.Add((byte)(color.R & (byte)1) != 0);
                }
            }
            BitArray result = new BitArray(tempResult.ToArray());

            Console.WriteLine(Converter.convertBitsToString(result));
        }
        
    }
}
