using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser : ITwisterParser
    {
        private readonly Func<ISymbolTable> CreateSymbolTableFunc;
        private readonly Func<IEnumerable<IToken>, ITokenMatcher> CreateTokenConsumerFunc;

        private ISymbolTable _symbolTable;
        private ITokenMatcher _matcher;
        private bool _hasMain;

        public TwisterParser(Func<ISymbolTable> createSymbolTableFunc,
            Func<IEnumerable<IToken>, ITokenMatcher> createTokenConsumerFunc)
        {
            CreateSymbolTableFunc = createSymbolTableFunc;
            CreateTokenConsumerFunc = createTokenConsumerFunc;
        }

        public INode Parse(IEnumerable<IToken> twisterTokens)
        {
            _symbolTable = CreateSymbolTableFunc();
            _matcher = CreateTokenConsumerFunc(twisterTokens);
            _hasMain = false;

            return Program();
        }

        /// <summary>
        /// program ::= func {func}
        /// </summary>
        private INode Program()
        {
            var functionBody = new List<IFuncNode<TwisterPrimitive>>();
            while (_matcher.IsNext<IToken>())
                functionBody.Add(Function());

            if (!_hasMain)
                throw new InvalidProgramException("Program entry point, 'func main', is missing");

            return new ProgramNode(functionBody.ToArray());
        }

        /// <summary>
        /// function ::= func identifier func_params {define_op type} colon funcbody
        /// </summary>
        private IFuncNode<TwisterPrimitive> Function()
        {
            var funcToken = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value == Keyword.Func);
            var idToken = _matcher.MatchAndGet<IValueToken<string>>();
            if (idToken.Value == "main")
                _hasMain = true; // TODO : Pattern check full main func signature

            var funcParams = FuncParams();

            PrimitiveType? type = null;
            if (_matcher.IsNext<DefineToken>())
            {
                _matcher.Match();
                var typeToken = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value.IsTypeKeyword());
                type = typeToken.Value.ToPrimitiveType();
            }

            _matcher.Match<ColonToken>();

            return new FuncNode<TwisterPrimitive>(idToken.Value, type, funcParams, FuncBody());
        }

        /// <summary>
        /// func_params ::= lsqrbrack params rsqrbrack
        /// </summary>
        private IList<ISymbolNode<TwisterPrimitive>> FuncParams()
        {
            _matcher.Match<LeftSquareBrackToken>();
            var @params = Params();
            _matcher.Match<RightSquareBrackToken>();
            return @params;
        }

        //params ::= param {comma param}
        private IList<ISymbolNode<TwisterPrimitive>> Params()
        {
            var @params = new List<ISymbolNode<TwisterPrimitive>> { Param() };

            while (_matcher.IsNext<CommaToken>())
            {
                _matcher.Match();
                @params.Add(Param());
            }

            return @params;
        }

        /// <summary>
        /// param ::= type colon identifier
        /// </summary>
        private ISymbolNode<TwisterPrimitive> Param()
        {
            var type = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value.IsTypeKeyword());
            _matcher.Match<ColonToken>();
            var identifier = _matcher.MatchAndGet<IValueToken<string>>();

            return new SymbolNode(SymbolKind.Parameter, identifier.Value,
                         new TwisterPrimitive(type.Value.ToPrimitiveType()));
        }

        /// <summary>
        /// func_body ::= lbrack body return_expr rbrack
        /// </summary>
        private IList<INode> FuncBody()
        {
            _matcher.Match<LeftBrackToken>();
            var body = Body();
            body.Add(ReturnExpression());
            return body;
        }

        /// <summary>
        /// body ::= expression | statement | lbrack {(expression | statement)} rbrack
        /// </summary>
        private IList<INode> Body()
        {
            var nodes = new List<INode>();
            var next = _matcher.PeekNext();
            if (next.Kind == TokenKind.LeftBrack)
            {
                _matcher.Match();
                while (!_matcher.IsNext<RightBrackToken>())
                {
                
                }
                _matcher.Match<RightBrackToken>();
            }

            // TODO
            return null;
        }
    }
}
