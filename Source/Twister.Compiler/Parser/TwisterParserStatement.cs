using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser
    {
        /// <summary>
        /// statement ::= (if_stmt | else_stmt | while_stmt | func_call | declaration | assignment | block) scolon
        /// </summary>
        public INode Statement()
        {
            INode statementNode;
            var peek = _matcher.Peek;

            switch (peek)
            {
                case LeftBrackToken leftBrackToken:
                    statementNode = Block();
                    break;
                case IValueToken<Keyword> keywordToken:
                    {
                        switch (keywordToken.Value)
                        {
                            case var t when t.IsTypeKeyword():
                            case var tt when tt == Keyword.Def:
                                statementNode = Declaration();
                                break;
                            case Keyword.If:
                                statementNode = If();
                                break;
                            case Keyword.Else:
                                statementNode = Else();
                                break;
                            case Keyword.While:
                                statementNode = While();
                                break;
                            case Keyword.Break:
                                statementNode = Break();
                                break;
                            case Keyword.Cont:
                                statementNode = Cont();
                                break;
                        }
                    }
                    break;
                case IValueToken<string> identifierToken:
                    {
                        var peekAfter = _matcher.PeekNext(2);
                        statementNode = peekAfter.Kind == TokenKind.RightSquareBrack
                            ? FuncCall()
                            : Assignment();
                    }
                    break;
            }

            _matcher.Match<SemiColonToken>();
            return null;
        }



        /// <summary>
        /// '{' { statement } '}'
        /// </summary>
        public IValueNode<INode> Block()
        {
            var statementList = new List<INode>();

            _matcher.Match<LeftBrackToken>();
            if (_matcher.IsNext<RightBrackToken>())
            {
                _matcher.Match();
                return null;
            }

            for (var snode = Statement(); snode != null || !(snode is TerminalNode); snode = Statement())
            {
                statementList.Add(snode);
            }

            _matcher.Match<RightBrackToken>();
            return null;
        }

        private INode If()
        {
            return null;
        }

        private INode Else()
        {
            return null;
        }

        private INode While()
        {
            return null;
        }

        private INode FuncCall()
        {
            return null;
        }

        private INode Cont()
        {
            return null;
        }

        private INode Break()
        {
            return null;
        }
    }
}
