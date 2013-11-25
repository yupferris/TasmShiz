using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class DpImm8Bit : Operand
    {
        public readonly int Bit;

        public DpImm8Bit(int bit)
        {
            Bit = bit;
        }
    }
}
