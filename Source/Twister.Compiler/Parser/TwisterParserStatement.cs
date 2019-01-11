using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Enum;
using System.Collections.Generic;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
        /// <summary>
        /// 
        /// </summary>
        public IValueNode<INode> Statement()
        {



            return null;
        }

        public INode SimpleStatement()
        {
            var peek = _matcher.PeekNext();
            if (peek.Kind == TokenKind.Semicolon)
            {
                _matcher.Match();
                return new TerminalNode();
            }

            switch (peek)
            {
                case IValueToken<Keyword> keyToken when keyToken is IValueToken<Keyword>:
                    {
                        switch (keyToken.Value)
                        {
                            case Keyword.If:
                                break;
                            case Keyword.Else:
                                break;
                            case Keyword.Func:
                                break;
                            case Keyword.Def:
                                break;
                            case Keyword.Return:
                                break;
                            case Keyword.While:
                                break;
                            case Keyword.Cont:
                                break;
                            case Keyword.Break:
                                break;
                            case Keyword.Bool:
                            case Keyword.Char:
                            case Keyword.Int:
                            case Keyword.UInt:
                            case Keyword.Float:
                            case Keyword.Str:
                            default:
                                throw new UnexpectedTokenException("Unexpected token")
                                { UnexpectedToken = keyToken };
                        }
                        break;
                    }
            }

            return null;
        }

        /// <summary>
        /// '{' { statement } '}'
        /// </summary>
        public IValueNode<INode> BlockStatement()
        {
            _matcher.Match<LeftBrackToken>();
            var statementList = new List<INode>();
            for (var snode = Statement(); snode != null || !(snode is TerminalNode); snode = Statement())
                statementList.Add(snode);

            _matcher.Match<RightBrackToken>();
            return null;
        }

    }
}
