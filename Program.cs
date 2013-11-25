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
                var instructions = new Parser().Process(File.ReadAllText(fileName), fileName);

                foreach (var instruction in instructions)
                {
                    var o = "new Instruction(\"" + instruction.Mnemonic + "\", " +
                        "0x" + instruction.Opcode.ToString("x2");
                    if (instruction.OperandsReversed)
                        o += ", InstructionOptions.ReverseOperands";
                    if (instruction.Operands.Count != 0)
                    {
                        foreach (var operand in instruction.Operands)
                        {
                            string tmp;
                            if (operand.Emit(out tmp))
                                o += ", " + tmp;
                        }
                    }
                    o += "),";

                    Console.WriteLine(o);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }
    }
}
