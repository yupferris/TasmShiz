using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class DpXImm8 : Operand
    {
        public override string Emit()
        {
            return "new Imm8PlusReg(regX)";
        }
    }
}
