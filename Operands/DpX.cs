using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class DpX : Operand
    {
        public readonly bool Increment;

        public DpX(bool increment)
        {
            Increment = increment;
        }

        public override string Emit()
        {
            return "regXParens" + (Increment ? "Inc" : "");
        }
    }
}
