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

        static char[] regIds = new[] { 'a', 'x', 'y' };

        void parseOperands()
        {
            do
            {
                saveState();
                if (tryParseRegisters() ||
                    tryParseRegistersParens() ||
                    tryParseImm8())
                    continue;
                restoreState();
                throw new Exception("Unrecognized operand " + prettyPrintSource(CurrentToken.Source));
            } while (Accept(TokenType.Comma));
        }

        bool tryParseRegisters()
        {
            foreach (var regId in regIds)
            {
                if (tryParseRegister(regId))
                {
                    _instruction.Operands.Add(new Register(regId));
                    return true;
                }
                restoreState();
            }
            return false;
        }

        bool tryParseRegistersParens()
        {
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

        bool tryParseImm8()
        {
            if (!Accept(TokenType.Hash))
                return false;
            if (!Accept(TokenType.Identifier))
                return false;
            if ((LastToken as Lexer.Identifier).Value.ToLower() != "i")
                return false;
            _instruction.Operands.Add(new Imm8());
            return true;
        }
    }
}
