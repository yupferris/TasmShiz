using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz
{
    enum Operand
    {

    }

    class Instruction
    {
        public string Mnemonic;
        public byte Opcode;
        public readonly List<Operand> Operands = new List<Operand>();
    }
}
