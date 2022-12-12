using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public enum OperationMode {lsb, parity}
    public enum EncryptionMode {encrypt, decrypt}
    internal class Parser
    {
        private string fileName { get; set; }
        public string path { get; set; }
        public bool exit { get; set; }
        public string secret { get; set; }
        public ISteganograph steganograph { get; set; }
        public EncryptionMode encryptionMode { get; set; }

        public Parser()
        {
            encryptionMode= EncryptionMode.encrypt;
            path= string.Empty;
            secret= string.Empty;
            exit = false;
            steganograph = new Parity(4);
            fileName =  @"/Filename.bmp";
        }
        public void parse()
        {
            string[] parameters =  Console.ReadLine().Split(' ');

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].ToLower().Trim();
                if (parameters[i] != " ")
                {
                    switch (parameters[i])
                    {
                        case "-h":
                            Console.WriteLine(StegProg.Properties.Resources.help_Message);
                            break;

                        case "-p":
                            try { path = parameters[i + 1].Trim('"'); }
                            catch (ArgumentNullException) { Console.WriteLine("No Path was passed! Use -p to pass a path or refer to -h for help. "); }
                            catch (ArgumentException) { Console.WriteLine("File needs to be of type: .txt!"); }
                            break;

                        case "lsb":
                            steganograph = new LSB();
                            break;

                        case "parity":
                            Console.Write("Please enter Size of Pixelblocks: ");
                            int size = Int16.Parse(Console.ReadLine());
                            steganograph= new Parity(size);
                            break;

                        case "encrypt":
                            encryptionMode= EncryptionMode.encrypt;
                            Console.Write("Please enter the Message you want to hide: ");
                            secret = Console.ReadLine();
                            break;

                        case "decrypt":
                            encryptionMode= EncryptionMode.decrypt;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public bool inputIsSufficient()
        {
            if(path == string.Empty)
            {
                Console.WriteLine("Please make sure to pass a path");
                return false;
            }

            if (secret == string.Empty && encryptionMode == EncryptionMode.encrypt)
            {
                Console.WriteLine("Please make sure to pass a message");
                return false;
            }

            return true;
            
        }
         public string getFilename()
         {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + fileName;
         }
    }
}
