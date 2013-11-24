using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fplcs;

namespace TasmShiz
{
    enum TokenType
    {
        Whitespace,

        Comment,

        Period,
        Colon,
        Hash,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        LessThan,
        GreaterThan,
        Comma,
        Slash,
        Bang,
        Plus,

        Identifier,
    }

    class Lexer : LexicalAnalyzer<TokenType>
    {
        public class Identifier : Token
        {
            public readonly string Value;

            public Identifier(string value, Source source)
                : base(TokenType.Identifier, source)
            {
                Value = value;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Identifier;
                return other != null && other.Value == Value;
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        public Lexer()
        {
            AddTokenDefinition(TokenType.Whitespace, "[ \\t\\n\\r]+", RegexOptions.None, (input, pos, length, source) => { return null; });

            AddTokenDefinition(TokenType.Comment, "//[^\\n]*", RegexOptions.None, (input, pos, length, source) => { return null; });

            AddTokenDefinition(TokenType.Period, "\\.");
            AddTokenDefinition(TokenType.Colon, ":");
            AddTokenDefinition(TokenType.Hash, "#");
            AddTokenDefinition(TokenType.LeftParen, "\\(");
            AddTokenDefinition(TokenType.RightParen, "\\)");
            AddTokenDefinition(TokenType.LeftBracket, "\\[");
            AddTokenDefinition(TokenType.RightBracket, "]");
            AddTokenDefinition(TokenType.LessThan, "<");
            AddTokenDefinition(TokenType.GreaterThan, ">");
            AddTokenDefinition(TokenType.Comma, ",");
            AddTokenDefinition(TokenType.Slash, "/");
            AddTokenDefinition(TokenType.Bang, "!");
            AddTokenDefinition(TokenType.Plus, "\\+");

            AddTokenDefinition(TokenType.Identifier, "[_a-zA-Z0-9]+", RegexOptions.None, (input, pos, length, source) => { return new Identifier(input.Substring(pos, length), source); });
        }
    }
}
