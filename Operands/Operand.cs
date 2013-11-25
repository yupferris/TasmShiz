using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    abstract class Operand
    {
        public virtual string Emit()
        {
            throw new NotImplementedException();
        }
    }
}
