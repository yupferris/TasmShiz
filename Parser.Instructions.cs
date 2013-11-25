using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz
{
    partial class Parser
    {
        bool tryParseInstruction()
        {
            expect(TokenType.Identifier);

            var instruction = new Instruction();
            instruction.Mnemonic = (_lastToken as Lexer.Identifier).Value;

            // arguments

            instruction.Opcode = parseByte();

            _instructions.Add(instruction);

            return true;
        }
    }
}
