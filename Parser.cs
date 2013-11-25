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
        public Lexer.Token CurrentToken, LastToken;

        int _savedTokenPos;
        Lexer.Token _savedCurrentToken, _savedLastToken;

        List<Instruction> _instructions;

        public IEnumerable<Instruction> Process(string input, string fileName)
        {
            _tokens = new Lexer().Process(input, fileName);
            _tokenPos = 0;
            CurrentToken = LastToken = null;
            FetchNextToken();

            _instructions = new List<Instruction>();

            while (_tokenPos < _tokens.Count)
            {
                if (!tryParseInstruction())
                    throw new Exception("Expecting instruction (" + prettyPrintSource(CurrentToken.Source) + ")");
            }

            return _instructions;
        }

        public void FetchNextToken()
        {
            if (_tokenPos > _tokens.Count)
                throw new Exception("Unexpected end of file (" + prettyPrintSource(LastToken.Source) + ")");
            LastToken = CurrentToken;
            CurrentToken = _tokenPos < _tokens.Count ? _tokens[_tokenPos] : null;
            _tokenPos++;
        }

        public bool Accept(TokenType type)
        {
            if (CurrentToken == null || CurrentToken.Type != type)
                return false;
            FetchNextToken();
            return true;
        }

        public void Expect(TokenType type)
        {
            if (!Accept(type))
                throw new Exception("Expected token type: " + type + "(" + (CurrentToken != null ? prettyPrintSource(CurrentToken.Source) : "") + ")");
        }

        string prettyPrintSource(Lexer.Source source)
        {
            return source.FileName + "[" + source.Line + ":" + source.Position + "]";
        }

        byte parseByte()
        {
            Expect(TokenType.Identifier);
            byte ret;
            if (!byte.TryParse((LastToken as Lexer.Identifier).Value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out ret))
                throw new Exception("Identifier wasn't a valid digit value " + prettyPrintSource(LastToken.Source));
            return ret;
        }

        void saveState()
        {
            _savedTokenPos = _tokenPos;
            _savedCurrentToken = CurrentToken;
            _savedLastToken = LastToken;
        }

        void restoreState()
        {
            _tokenPos = _savedTokenPos;
            CurrentToken = _savedCurrentToken;
            LastToken = _savedLastToken;
        }
    }
}
