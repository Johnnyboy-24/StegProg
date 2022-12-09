using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public class Parity : ISteganograph
    {
        public Parity(int codeblockLength) 
        {
            codeblock = codeblockLength;
        }
        int codeblock;
        public Bitmap Hide(Bitmap image, BitArray secret)
        {
            Color color;
            int indexer = 0;
            byte alphaValue;
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            int counter = 0;
            int evenNumbers = 0;
            bool parity;

            
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    counter++;
                    color = image.GetPixel(i, j);
                    if (indexer/codeblock < secret.Length)
                    {
                        if (lsbIsFalse(color)) { evenNumbers++; }    

                        if (counter == codeblock)
                        {
                            parity = (evenNumbers % 2 == 0);
                            bool somebool = (parity != secret[indexer / codeblock]);
                            if (parity != secret[indexer/codeblock]) { color = changeParity(color); }
                            counter= 0;
                            evenNumbers= 0;
                        }

                    }                    
                    else
                    {
                        color = makeEven(color);
                    }
                    
                    newImage.SetPixel(i, j, color);

                    indexer++;
                }
            }
            return newImage;
        }

        private Color changeParity(Color actual)
        {
            if (lsbIsFalse(actual))             
            { 
                return makeUneven(actual); 
            }
            else return makeEven(actual);
        }

        private bool lsbIsFalse(Color color)
        {
            return ((byte)(color.R & (byte)1) == 0);
        }


        private Color makeUneven(Color color)
        {
            byte intendedColorValue = (byte)(color.R | 1);
            return Color.FromArgb( color.A, intendedColorValue, color.G, color.B);
        }
        private Color makeEven(Color color)
        { 
            byte intendedColorValue = (byte)((color.R & 254));
            return Color.FromArgb( color.A, intendedColorValue, color.G, color.B);
        }

        

        public void Unvail(Bitmap image)
        {
            List<bool> tempResult = new List<bool>();
            Color color;
            int counter = 0;
            int evenNumbers = 0;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    counter++;
                    color = image.GetPixel(i, j);
                    if (lsbIsFalse(color)) { evenNumbers++; }
                    if(counter == codeblock)
                    {
                        if(evenNumbers % 2 == 0)
                        {
                            tempResult.Add(true);
                        }
                        else
                        {
                            tempResult.Add(false);
                        } 
                        counter= 0;
                        evenNumbers= 0;
                    }
                }
            }
            BitArray result = new BitArray(tempResult.ToArray());

            Console.WriteLine(Converter.convertBitsToString(result));
        }
    }
}
