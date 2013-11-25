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
            instruction.Mnemonic = (_lastToken as Lexer.Identifier).Value.ToLower();

            // arguments

            instruction.Opcode = parseByte();

            instruction.OperandsReversed =
                instruction.Operands.Count != 0 &&
                instruction.Mnemonic != "bbc" &&
                instruction.Mnemonic != "bbs" &&
                instruction.Mnemonic != "cbne" &&
                instruction.Mnemonic != "dbnz";

            _instructions.Add(instruction);

            return true;
        }
    }
}
