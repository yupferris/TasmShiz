using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    abstract class Operand
    {
        public virtual bool Emit(out string o)
        {
            throw new NotImplementedException();
        }
    }
}
