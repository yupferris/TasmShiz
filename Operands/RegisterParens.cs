using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    class RegisterParens : Register
    {
        public RegisterParens(string value)
            : base(value)
        {
        }

        public override string Emit()
        {
            return "reg" + Value.ToUpper() + "Parens";
        }
    }
}
