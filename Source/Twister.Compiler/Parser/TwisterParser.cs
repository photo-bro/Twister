using System;
using System.Collections.Generic;
using System.Linq;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Lexer.Token;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;
using Twister.Compiler.Parser.Symbol;

namespace Twister.Compiler.Parser
{
    public partial class TwisterParser : ITwisterParser
    {
        private readonly Func<IEnumerable<IToken>, ITokenMatcher> _createTokenMatcher;
        private readonly IExpressionParser _expressionParser;

        private ITokenMatcher _matcher;
        private IScopeManager _scopeManager;
        private bool _hasMain;


        public TwisterParser(
            Func<IEnumerable<IToken>, ITokenMatcher> createTokenMatcher,
            IExpressionParser expressionParser)
        {
            _createTokenMatcher = createTokenMatcher;
            _expressionParser = expressionParser;
        }

        private void SetupParser(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new InvalidProgramException("Source text is empty");

            _matcher = _createTokenMatcher(tokens);
            _scopeManager = new ScopeManager();

            _hasMain = false;
        }

        public INode ParseProgram(IEnumerable<IToken> twisterTokens)
        {
            SetupParser(twisterTokens);

            return Program();
        }

        public INode ParseExpression(IEnumerable<IToken> twisterTokens)
        {
            SetupParser(twisterTokens);

            return Expression();
        }

        /// <summary>
        /// program ::= func {func}
        /// </summary>
        private INode Program()
        {
            var functionBody = new List<IFuncNode<TwisterPrimitive>>();
            while (_matcher.Peek.Kind != TokenKind.None)
                functionBody.Add(Function());

            if (!_hasMain)
                throw new InvalidProgramException("Program entry point, 'func main', is missing");

            return new ProgramNode(functionBody.Cast<INode>().ToList());
        }

        /// <summary>
        /// function ::= func identifier {func_params} {define_op type} colon func_body
        /// </summary>
        private IFuncNode<TwisterPrimitive> Function()
        {
            var funcToken = _matcher.MatchAndGet<IValueToken<Keyword>>(t => t.Value == Keyword.Func);
            var funcName = _matcher.MatchAndGet<IValueToken<string>>().Value;

            if (funcName == "main")
                _hasMain = true; // TODO : Pattern check full main func signature

            var scope = _scopeManager.NewScope();
            if (_matcher.IsNext<RightBrackToken>())
            {
                var funcParams = FuncParams();
                scope.AddSymbols(funcParams);
            }
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

            var funcSymbol = new FuncSymbol(funcName, type, SymbolAttribute.None, null /* TODO */);
            _scopeManager.ActiveScope.AddSymbol(funcSymbol);

            return new FuncNode<TwisterPrimitive>(funcName, type, scope, body);
        }

        /// <summary>
        /// func_params ::= lsqrbrack params rsqrbrack
        /// </summary>
        private IList<ISymbol> FuncParams()
        {
            _matcher.Match<LeftSquareBrackToken>();
            var parameters = Params();
            _matcher.Match<RightSquareBrackToken>();
            return parameters;
        }

        //params ::= param {comma param}
        private IList<ISymbol> Params()
        {
            var parameters = new List<ISymbol> { Param() };

            while (_matcher.IsNext<CommaToken>())
            {
                _matcher.Match();
                parameters.Add(Param());
            }

            return parameters;
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
            var returnExpression = Expression();
            body.Add(returnExpression);
            return body;
        }

        /// <summary>
        /// body ::=  statement | lbrack {statement} rbrack
        /// </summary>
        private IList<INode> Body()
        {
            var nodes = new List<INode>();
            if (_matcher.IsNext<LeftBrackToken>())
            {
                _matcher.Match();
                while (!_matcher.IsNext<RightBrackToken>())
                {
                    nodes.Add(Statement());
                }
                _matcher.Match<RightBrackToken>();
            }
            else
            {
                nodes.Add(Statement());
            }

            return nodes;
        }
    }
}
