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

                using (var output = File.OpenWrite(args[1]))
                {
                    using (var writer = new StreamWriter(output))
                    {
                        foreach (var instruction in instructions.Where(x => x.Opcode != 0xfa))
                            writeInstruction(instruction, writer);
                        foreach (var instruction in instructions.Where(x => x.Opcode == 0xfa))
                            writeInstruction(instruction, writer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        static void writeInstruction(Instruction instruction, StreamWriter writer)
        {
            bool isCommentedOut = false;

            var o = "new Instruction(\"" + instruction.Mnemonic + "\", " +
                "0x" + instruction.Opcode.ToString("x2");
            if (instruction.OperandsReversed)
                o += ", InstructionOptions.ReverseOperands";
            if (instruction.Operands.Count != 0)
            {
                foreach (var operand in instruction.Operands)
                {
                    o += ", " + operand.Emit();

                    if (operand is Operands.Abs13B3)
                        isCommentedOut = true;
                }
            }
            o += "),";

            if (isCommentedOut)
                o = "//" + o;

            writer.WriteLine("                    " + o);
        }
    }
}
