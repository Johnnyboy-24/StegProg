using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public enum OperationMode {lsb, parity}
    public enum EncryptionMode {encrypt, decrypt}
    internal class Parser
    {
        public string path { get; set; }
        public bool exit { get; set; }
        public OperationMode operationMode { get; set; }
        public EncryptionMode encryptionMode { get; set; }

        public Parser()
        {
            exit = false;
            path = @"C:\Users\b190\Pictures\M113086-Neapolitanische_Pizza_112317_Titelbild-Q75-750.jpg";
        }
        public void parse (string[] parameters)
        { 
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = parameters[i].ToLower().Trim();
                if (parameters[i] != "")
                {
                    switch (parameters[i])
                    {
                        //case "-h":
                        //    Console.WriteLine(Solver.Properties.Resources.help_Message);
                        //    break;
                        case "-p":
                            try { path = @parameters[i + 1]; }
                            catch (ArgumentNullException) { Console.WriteLine("No Path was passed! Use -p to pass a path or refer to -h for help. "); }
                            catch (ArgumentException) { Console.WriteLine("File needs to be of type: .txt!"); }
                            break;
                        default:
                            break;
                        case "-e":
                            if (parameters[i+1] == "encrypt") { encryptionMode = EncryptionMode.encrypt;}
                            if (parameters[i + 1] == "decrypt") { encryptionMode|= EncryptionMode.decrypt;}
                            else { Console.WriteLine("Ungültiger Verschlüsselungsmodus: Bitte wähle aus encrypt oder decrypt"); }
                            break;
                        case "-o":
                            if (parameters[i+1] == "lsb") { operationMode= OperationMode.lsb; }
                            if (parameters[i+1] == "parity") { operationMode= OperationMode.parity; }
                            else { Console.WriteLine("Ungültiger Operationsmodus: Bitte wähle aus lsb oder parity"); }
                            break;
                    }
                }

            }
        
        }
    }
}
