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
                    tryParseImm8() ||
                    tryParseAbs16X() ||
                    tryParseAbs16Y() ||
                    tryParseAbs16() ||
                    tryParseDpXImm8() ||
                    tryParseDpYInd() ||
                    tryParseDpXInd() ||
                    tryParseDpImm8())
                    continue;
                restoreState();
                throw new Exception("Unrecognized operand " + prettyPrintSource(CurrentToken.Source));
            } while (Accept(TokenType.Comma));
        }

        bool tryParseRegisters()
        {
            foreach (var regId in regIds)
            {
                if (tryParseChar(regId))
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

        bool tryParseImm8()
        {
            if (Accept(TokenType.Hash) &&
                tryParseChar('i'))
            {
                _instruction.Operands.Add(new Imm8());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseAbs16X()
        {
            if (Accept(TokenType.Bang) &&
                tryParseChar('a') &&
                Accept(TokenType.Plus) &&
                tryParseChar('x'))
            {
                _instruction.Operands.Add(new Abs16X());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseAbs16Y()
        {
            if (Accept(TokenType.Bang) &&
                tryParseChar('a') &&
                Accept(TokenType.Plus) &&
                tryParseChar('y'))
            {
                _instruction.Operands.Add(new Abs16Y());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseAbs16()
        {
            if (Accept(TokenType.Bang) &&
                tryParseChar('a'))
            {
                _instruction.Operands.Add(new Abs16());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseDpXImm8()
        {
            if (tryParseChar('d') &&
                Accept(TokenType.Plus) &&
                tryParseChar('x'))
            {
                _instruction.Operands.Add(new DpXImm8());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseDpYInd()
        {
            if (Accept(TokenType.LeftBracket) &&
                tryParseChar('d') &&
                Accept(TokenType.RightBracket) &&
                Accept(TokenType.Plus) &&
                tryParseChar('y'))
            {
                _instruction.Operands.Add(new DpYInd());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseDpXInd()
        {
            if (Accept(TokenType.LeftBracket) &&
                tryParseChar('d') &&
                Accept(TokenType.Plus) &&
                tryParseChar('x') &&
                Accept(TokenType.RightBracket))
            {
                _instruction.Operands.Add(new DpXInd());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseDpImm8()
        {
            if (tryParseChar('d'))
            {
                _instruction.Operands.Add(new DpImm8());
                return true;
            }
            return false;
        }

        bool tryParseRegisterParens(char reg)
        {
            if (!Accept(TokenType.LeftParen))
                return false;
            if (!tryParseChar(reg))
                return false;
            if (!Accept(TokenType.RightParen))
                return false;
            return true;
        }

        bool tryParseChar(char c)
        {
            if (!Accept(TokenType.Identifier))
                return false;
            var id = (LastToken as Lexer.Identifier).Value.ToLower();
            if (id.Length != 1 || id.First() != c)
                return false;
            return true;
        }
    }
}
