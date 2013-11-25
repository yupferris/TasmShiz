using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class Abs13B3 : Operand
    {
        public readonly bool Not;

        public Abs13B3(bool not)
        {
            Not = not;
        }
    }
}
