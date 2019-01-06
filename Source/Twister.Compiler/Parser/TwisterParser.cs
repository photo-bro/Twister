using System;
using System.Linq;
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
    public class TwisterParser : ITwisterParser
    {
        private readonly Func<ISymbolTable> CreateSymbolTableFunc;
        private readonly Func<IEnumerable<IToken>, ITokenMatcher> CreateTokenConsumerFunc;

        private ISymbolTable _symbolTable;
        private ITokenMatcher _consumer;
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
            _consumer = CreateTokenConsumerFunc(twisterTokens);
            _hasMain = false;

            return Program();
        }

        /// <summary>
        /// program ::= func {func}
        /// </summary>
        private INode Program()
        {
            var functionBody = new List<IFuncNode<TwisterPrimitive>>();
            while (_consumer.PeekNext<IToken>() != null)
                functionBody.Add(Function());

            if (!_hasMain)
                throw new InvalidProgramException("Program entry point, 'func main', is missing");

            return new ProgramNode(functionBody.ToArray());
        }

        /// <summary>
        /// function ::= func identifier lsqrbrack params rsqrbrack {define_op type} colon lbrack
        ///  {(expression | statement)} return_expr rbrack
        /// </summary>
        private IFuncNode<TwisterPrimitive> Function()
        {
            var funcToken = _consumer.MatchAndGet<IValueToken<Keyword>>(t => t.Value == Keyword.Func);
            var idToken = _consumer.MatchAndGet<IValueToken<string>>();
            if (idToken.Value == "main")
                _hasMain = true; // TODO : Pattern check full main func signature

            _consumer.Match<LeftSquareBrackToken>(); 

            var @params = Params();

            _consumer.Match<RightSquareBrackToken>();

            PrimitiveType? type = null;
            if (_consumer.PeekNext<DefineToken>() != null)
            {
                _consumer.Match();
                var typeToken = _consumer.MatchAndGet<IValueToken<Keyword>>(t => t.Value.IsTypeKeyword());
                type = typeToken.Value.ToPrimitiveType();
            }

            _consumer.Match<ColonToken>();
            _consumer.Match<LeftBrackToken>();

            var body = Body();

            _consumer.MatchAndGet<IValueToken<Keyword>>(t => t.Value == Keyword.Return);

            var returnExpression = Expression<TwisterPrimitive>();

            _consumer.Match<RightBrackToken>();

            return new FuncNode<TwisterPrimitive>(idToken.Value, type, @params, body, returnExpression);
        }

        private ISymbolNode<TwisterPrimitive>[] Params()
        {

            return null;
        }

        private INode[] Body()
        {

            return null;
        }

        private IExpressionNode<T> Expression<T>()
        {
            return null;
        }

        private INode Assignment(IExpressionNode<TwisterPrimitive> expression)
        {

            return null;
        }
    }
}
