using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasmShiz
{
    partial class Parser
    {
        List<Lexer.Token> _tokens;
        int _tokenPos;
        Lexer.Token _currentToken, _lastToken;

        List<Instruction> _instructions;

        public IEnumerable<Instruction> Process(string input, string fileName)
        {
            _tokens = new Lexer().Process(input, fileName);
            _tokenPos = 0;
            _currentToken = _lastToken = null;
            fetchNextToken();

            _instructions = new List<Instruction>();

            while (_tokenPos < _tokens.Count)
            {
                if (!tryParseInstruction())
                    throw new Exception("Expecting instruction (" + prettyPrintSource(_currentToken.Source) + ")");
            }

            return _instructions;
        }

        void fetchNextToken()
        {
            if (_tokenPos > _tokens.Count)
                throw new Exception("Unexpected end of file (" + prettyPrintSource(_lastToken.Source) + ")");
            _lastToken = _currentToken;
            _currentToken = _tokenPos < _tokens.Count ? _tokens[_tokenPos] : null;
            _tokenPos++;
        }

        bool accept(TokenType type)
        {
            if (_currentToken == null || _currentToken.Type != type)
                return false;
            fetchNextToken();
            return true;
        }

        void expect(TokenType type)
        {
            if (!accept(type))
                throw new Exception("Expected token type: " + type + "(" + (_currentToken != null ? prettyPrintSource(_currentToken.Source) : "") + ")");
        }

        string prettyPrintSource(Lexer.Source source)
        {
            return source.FileName + "[" + source.Line + ":" + source.Position + "]";
        }

        byte parseByte()
        {
            expect(TokenType.Identifier);
            byte ret;
            if (!byte.TryParse((_lastToken as Lexer.Identifier).Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out ret))
                throw new Exception("Identifier wasn't a valid digit value " + prettyPrintSource(_lastToken.Source));
            return ret;
        }
    }
}
