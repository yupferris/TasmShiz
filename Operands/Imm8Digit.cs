using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class Imm8Digit : Operand
    {
        public readonly int Value;

        public Imm8Digit(int value)
        {
            Value = value;
        }
    }
}
