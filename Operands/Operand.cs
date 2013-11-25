using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz.Operands
{
    abstract class Operand
    {
        abstract bool Parse();

        virtual bool Emit()
        {
            throw new NotImplementedException();
        }
    }
}
