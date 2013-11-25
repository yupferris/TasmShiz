using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class Register : Operand
    {
        public readonly string Value;

        public Register(string value)
        {
            Value = value;
        }

        public override string Emit()
        {
            return "reg" + Value.ToUpper();
        }
    }
}
