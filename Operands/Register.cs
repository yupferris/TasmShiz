using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class Register : Operand
    {
        public readonly char Value;

        public Register(char value)
        {
            Value = value;
        }
    }
}
