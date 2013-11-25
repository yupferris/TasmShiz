using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasmShiz.Operands;

namespace TasmShiz
{
    partial class Parser
    {
        Instruction _instruction;

        bool tryParseInstruction()
        {
            Expect(TokenType.Identifier);

            _instruction = new Instruction();
            _instruction.Mnemonic = (LastToken as Lexer.Identifier).Value.ToLower();

            parseOperands();

            _instruction.Opcode = parseByte();

            _instruction.OperandsReversed =
                _instruction.Operands.Count != 0 &&
                _instruction.Mnemonic != "bbc" &&
                _instruction.Mnemonic != "bbs" &&
                _instruction.Mnemonic != "cbne" &&
                _instruction.Mnemonic != "dbnz";

            _instructions.Add(_instruction);

            return true;
        }

        static char[] regIds = new[] { 'x', 'y' };

        void parseOperands()
        {
            do
            {
                if (tryParseRegistersParens())
                    continue;
                throw new Exception("Unrecognized operand " + prettyPrintSource(CurrentToken.Source));
            } while (Accept(TokenType.Comma));
        }

        bool tryParseRegistersParens()
        {
            saveState();
            foreach (var regId in regIds)
            {
                if (tryParseRegisterParens(regId))
                {
                    _instruction.Operands.Add(new RegisterParens(regId));
                    return true;
                }
                restoreState();
            }
            return false;
        }

        bool tryParseRegisterParens(char reg)
        {
            if (!Accept(TokenType.LeftParen))
                return false;
            if (!tryParseRegister(reg))
                return false;
            if (!Accept(TokenType.RightParen))
                return false;
            return true;
        }

        bool tryParseRegister(char reg)
        {
            if (!Accept(TokenType.Identifier))
                return false;
            var id = (LastToken as Lexer.Identifier).Value.ToLower();
            if (id.Length != 1 || id.First() != reg)
                return false;
            return true;
        }
    }
}
