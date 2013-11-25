using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasmShiz.Operands;

namespace TasmShiz
{
    class Instruction
    {
        public string Mnemonic;
        public byte Opcode;

        public bool OperandsReversed;

        public readonly List<Operand> Operands = new List<Operand>();
    }
}
