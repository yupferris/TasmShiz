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
                _instruction.Operands.Count == 2 &&
                _instruction.Mnemonic != "bbc" &&
                _instruction.Mnemonic != "bbs" &&
                _instruction.Mnemonic != "cbne" &&
                _instruction.Mnemonic != "dbnz";

            _instructions.Add(_instruction);

            return true;
        }

        static string[] regIds = new[] { "a", "x", "y", "ya", "c", "sp", "psw" };

        void parseOperands()
        {
            do
            {
                saveState();
                if (tryParseImm8() ||
                    tryParseRel8() ||
                    tryParseAbs16XInd() ||
                    tryParseAbs16X() ||
                    tryParseAbs16Y() ||
                    tryParseAbs16() ||
                    tryParseDpX() ||
                    tryParseDpXImm8() ||
                    tryParseDpYImm8() ||
                    tryParseDpYInd() ||
                    tryParseDpXInd() ||
                    tryParseDpImm8Bit() ||
                    tryParseDpImm8() ||
                    tryParseAbs13B3() ||
                    tryParseRegisters() ||
                    tryParseRegistersParens() ||
                    tryParseImm8Digit())
                    continue;
                restoreState();
            } while (Accept(TokenType.Comma));
        }

        bool tryParseRegisters()
        {
            foreach (var regId in regIds)
            {
                if (tryParseString(regId))
                {
                    _instruction.Operands.Add(new Register(regId != "psw" ? regId : "p"));
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
            if ((Accept(TokenType.Hash) &&
                tryParseChar('i')) ||
                tryParseChar('u'))
            {
                _instruction.Operands.Add(new Imm8());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseRel8()
        {
            if (tryParseChar('r'))
            {
                _instruction.Operands.Add(new Imm8());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseAbs16XInd()
        {
            if (Accept(TokenType.LeftBracket) &&
                Accept(TokenType.Bang) &&
                tryParseChar('a') &&
                Accept(TokenType.Plus) &&
                tryParseChar('x') &&
                Accept(TokenType.RightBracket))
            {
                _instruction.Operands.Add(new Abs16XInd());
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

        bool tryParseDpX()
        {
            if (Accept(TokenType.LeftParen) &&
                tryParseChar('x') &&
                Accept(TokenType.RightParen))
            {
                _instruction.Operands.Add(new DpX(Accept(TokenType.Plus)));
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

        bool tryParseDpYImm8()
        {
            if (tryParseChar('d') &&
                Accept(TokenType.Plus) &&
                tryParseChar('y'))
            {
                _instruction.Operands.Add(new DpYImm8());
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

        bool tryParseDpImm8Bit()
        {
            if (tryParseChar('d') && Accept(TokenType.Period))
            {
                _instruction.Operands.Add(new DpImm8Bit(parseByte()));
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseDpImm8()
        {
            if (tryParseChar('d') || tryParseString("dd") || tryParseString("ds"))
            {
                _instruction.Operands.Add(new DpImm8());
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseAbs13B3()
        {
            bool not = Accept(TokenType.Slash);
            if (tryParseChar('m') &&
                Accept(TokenType.Period) &&
                tryParseChar('b'))
            {
                _instruction.Operands.Add(new Abs13B3(not));
                return true;
            }
            restoreState();
            return false;
        }

        bool tryParseRegisterParens(string reg)
        {
            if (!Accept(TokenType.LeftParen))
                return false;
            if (!tryParseString(reg))
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
            {
                restoreState();
                return false;
            }
            return true;
        }

        bool tryParseString(string s)
        {
            if (Accept(TokenType.Identifier) &&
                (LastToken as Lexer.Identifier).Value.ToLower() == s)
                return true;
            restoreState();
            return false;
        }

        bool tryParseImm8Digit()
        {
            if (tryParseString("penis") && Accept(TokenType.Period) && Accept(TokenType.Identifier))
            {
                var id = (LastToken as Lexer.Identifier).Value;
                int value;
                if (int.TryParse(id, out value) && value >= 0 && value < 16)
                {
                    _instruction.Operands.Add(new Imm8Digit(value));
                    return true;
                }
            }
            restoreState();
            return false;
        }
    }
}
