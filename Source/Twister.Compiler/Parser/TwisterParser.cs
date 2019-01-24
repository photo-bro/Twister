using System;
using System.Collections.Generic;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Symbol;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser : ITwisterParser
    {
        private readonly Func<IEnumerable<IToken>, ITokenMatcher> CreateTokenMatcherFunc;

        private ITokenMatcher _matcher;
        private IScopeManager _scopeManager;


        private bool _hasMain;

        public TwisterParser(Func<IEnumerable<IToken>, ITokenMatcher> createTokenMatcherFunc)
        {
            CreateTokenMatcherFunc = createTokenMatcherFunc;
        }

        private void SetupParser()
        {
            _scopeManager = new ScopeManager();

            _hasMain = false;
        }

        public INode ParseProgram(IEnumerable<IToken> twisterTokens)
        {
            _matcher = CreateTokenMatcherFunc(twisterTokens);
            SetupParser();

            return Program(); // TODO !!! Set back to program and find a proper way to unit test these methods
        }

        public INode ParseExpression(IEnumerable<IToken> twisterTokens)
        {
            _matcher = CreateTokenMatcherFunc(twisterTokens);
            SetupParser();

            return Expression();
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

            return new ProgramNode((IList<INode>)functionBody);
        }

        /// <summary>
        /// function ::= func identifier func_params {define_op type} colon funcbody
        /// </summary>
        private IFuncNode<TwisterPrimitive> Function()
        {
            var funcToken = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value == Keyword.Func);
            var idToken = _matcher.MatchAndGet<IValueToken<string>>();
            var funcName = idToken.Value;
            if (idToken.Value == "main")
                _hasMain = true; // TODO : Pattern check full main func signature

            var scope = _scopeManager.NewScope();
            scope.AddSymbols(FuncParams());
            _scopeManager.RemoveBottomScope();

            var type = TwisterType.Void;
            if (_matcher.IsNext<DefineToken>())
            {
                _matcher.Match();
                var typeToken = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value.IsTypeKeyword());
                type = typeToken.Value.ToTwisterType();
            }

            _matcher.Match<ColonToken>();

            var body = FuncBody();

            _scopeManager.ActiveScope.AddSymbol(new FuncSymbol()); // TODO - Flesh out FuncSymbol

            return new FuncNode<TwisterPrimitive>(idToken.Value, type, scope, body);
        }

        /// <summary>
        /// func_params ::= lsqrbrack params rsqrbrack
        /// </summary>
        private IList<ISymbol> FuncParams()
        {
            _matcher.Match<LeftSquareBrackToken>();
            var @params = Params();
            _matcher.Match<RightSquareBrackToken>();
            return @params;
        }

        //params ::= param {comma param}
        private IList<ISymbol> Params()
        {
            var @params = new List<ISymbol> { Param() };

            while (_matcher.IsNext<CommaToken>())
            {
                _matcher.Match();
                @params.Add(Param());
            }

            return @params;
        }

        /// <summary>
        /// param ::= {ref} type colon identifier
        /// </summary>
        private ISymbol Param()
        {
            var attributes = SymbolAttribute.FuncParam;
            var tk = _matcher.MatchAndGet<IValueToken<Keyword>>();
            if (tk.Value == Keyword.Ref)
            {
                attributes |= SymbolAttribute.Reference;
                _matcher.Match();
                tk = _matcher.MatchAndGet<IValueToken<Keyword>>();
            }

            if (!tk.Value.IsTypeKeyword())
                throw new UnexpectedTokenException("Expecting type keyword")
                { UnexpectedToken = tk };

            _matcher.Match<ColonToken>();
            var identifier = _matcher.MatchAndGet<IValueToken<string>>();

            return new BasicSymbol(
                 identifier: identifier.Value,
                 kind: SymbolKind.Variable,
                 attributes: attributes,
                 dataType: tk.Value.ToTwisterType(),
                 value: null);
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
            var next = _matcher.Peek;
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
