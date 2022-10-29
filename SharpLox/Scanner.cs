using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class Scanner
    {
        private string Source { get; init; }
        public List<Token> Tokens { get; set; } = new();

        public int Start { get; set; } = 0;
        public int Current { get; set; } = 0;
        public int Line { get; set; } = 1;

        private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            {"true", TokenType.TRUE },
            {"false", TokenType.FALSE },
            {"and", TokenType.AND },
            {"or", TokenType.OR },
            {"if", TokenType.IF },
            {"else", TokenType.ELSE },
            {"for", TokenType.FOR },
            {"while", TokenType.WHILE },
            {"class", TokenType.CLASS },
            {"fun", TokenType.FUN },
            {"return", TokenType.RETURN },
            {"super", TokenType.SUPER },
            {"this", TokenType.THIS },
            {"var", TokenType.VAR },
            {"print", TokenType.PRINT },
            {"nil", TokenType.NIL }
        };

        public Scanner(string source)
        {
            Source = source;
        }

        public List<Token> ScanTokens()
        {
            while(!IsAtEnd())
            {
                Start = Current;
                ScanToken();
            }

            return Tokens;
        }

        private void ScanToken()
        {
            var c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '!':
                    AddToken(NextIs('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(NextIs('=') ? TokenType.EQUAL_EQUAL: TokenType.EQUAL);
                    break;
                case '>':
                    AddToken(NextIs('=') ? TokenType.GREATER_EQUAL: TokenType.GREATER);
                    break;
                case '<':
                    AddToken(NextIs('=') ? TokenType.LESS_EQUAL: TokenType.LESS);
                    break;
                case '/':
                    if (NextIs('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                        break;
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    // Ignored
                    break;

                case '\n':
                    Line++;
                    break;

                case '"':
                    ScanString(); 
                    break;

                default:
                    if (IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        SharpLox.Error(Line, $"Unknown character '{c}'");
                    }
                    break;
            }
        }

        private void ScanIdentifier()
        {
            while(IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            var text = Source[Start..Current];
            var keywordFound = _keywords.TryGetValue(text, out var type);
            if (!keywordFound) type = TokenType.IDENTIFIER;

            AddToken(type);
        }

        private void ScanNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.NUMBER, double.Parse(Source[Start..Current]));
        }

        private void ScanString()
        {
            // TODO: Implement escape sequences

            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    Line++;
                }
                Advance();
            }

            // Handling reasons of broke the loop
            if (IsAtEnd())
            {
                SharpLox.Error(Line, "Unterminated string literal");
                return;
            }

            // Closing quote
            Advance();

            var value = Source[(Start+1)..(Current-1)];
            AddToken(TokenType.STRING, value);
        }

        private void AddToken(TokenType type) => AddToken(type, null);

        private void AddToken(TokenType type, object? literal)
        {
            Tokens.Add(new Token(type, Source[Start..Current], literal, Line));
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsDigit(char c)
        {
            return c > '0' && c < '9';
        }

        private char Peek()
        {
            return IsAtEnd() ? '\0' : Source[Current];
        }

        private char PeekNext()
        {
            if (Current + 1 >= Source.Length) return '\0';
            return Source[Current + 1];
        }

        private bool NextIs(char expected)
        {
            if (IsAtEnd()) return false;

            if (Source[Current] != expected) return false;

            Current++;
            return true;
        }

        // Returns element at Current and increases it
        private char Advance() => Source[Current++];

        private bool IsAtEnd() => Current >= Source.Length;
    }
}
