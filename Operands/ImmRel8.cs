using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class ImmRel8 : Operand
    {
        public override string Emit()
        {
            return "new ImmRel8()";
        }
    }
}
