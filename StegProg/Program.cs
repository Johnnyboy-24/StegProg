using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
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
            Parser parser = new Parser();
            Console.Write(StegProg.Properties.Resources.Welcome_Message);
            
            while (!parser.exit)
            {            
                parser.parse();
                if (parser.inputIsSufficient())
                {
                    runBAasedOnParserParams(parser);
                }
                
            }
        }
        static void runBAasedOnParserParams(Parser parser)
        {
            if(parser.encryptionMode == EncryptionMode.encrypt)
            {
                Bitmap result = parser.steganograph.Hide(Utility.GenerateBitmap(parser.path), Utility.getBits(parser.secret));
                Utility.saveImage(result, parser.fileName);
            }
            if(parser.encryptionMode == EncryptionMode.decrypt)
            {
                parser.steganograph.Unvail(Utility.GenerateBitmap(parser.path));
            }
        }
    }
}
