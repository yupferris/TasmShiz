﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class Abs16XInd : Operand
    {
        public override string Emit()
        {
            return "new Imm16IndPlusReg(regX)";
        }
    }
}
