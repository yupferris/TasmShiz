using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TasmShiz
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileName = args[0];
                var tokens = new Lexer().Process(File.ReadAllText(fileName), fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }
    }
}
