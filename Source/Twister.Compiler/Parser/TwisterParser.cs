using System;
using System.Linq;
using System.Collections.Generic;
using Twister.Compiler.Common.Interface;
using Twister.Compiler.Lexer.Enum;
using Twister.Compiler.Lexer.Interface;
using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser
{
    public class TwisterParser : ITwisterParser
    {
        private readonly Func<ISymbolTable> CreateSymbolTableFunc;
        private readonly Func<IEnumerable<IToken>, ITokenScanner> CreateTokenScannerFunc;

        private ISymbolTable _symbolTable;
        private ITokenScanner _tokenScanner;
        private bool _hasMain;

        public TwisterParser(Func<ISymbolTable> createSymbolTableFunc,
            Func<IEnumerable<IToken>, ITokenScanner> createTokenScannerFunc)
        {
            CreateSymbolTableFunc = createSymbolTableFunc;
            CreateTokenScannerFunc = createTokenScannerFunc;
        }

        public INode Parse(IEnumerable<IToken> twisterTokens)
        {
            _symbolTable = CreateSymbolTableFunc();
            _tokenScanner = CreateTokenScannerFunc(twisterTokens);
            _hasMain = false;

            var head = Program();

            if (!_hasMain)
                throw new InvalidProgramException("Program entry point, 'func main', is missing");

            return head;
        }

        private INode Program()
        {
            var functionBody = new List<IFuncNode<TwisterPrimitive>>();
            while (_tokenScanner.PeekNext<IToken>() != null)
                functionBody.Add(Function());

            return new ProgramNode(functionBody.ToArray());
        }

        private IFuncNode<TwisterPrimitive> Function()
        {
            var funcToken = _tokenScanner.ScanNext<IValueToken<Keyword>>();
            if (funcToken != null && funcToken.Value == Keyword.Func)
            {
                var idToken = _tokenScanner.ScanNext<IValueToken<string>>();
                if (idToken.Value == "main")
                    _hasMain = true;

                _tokenScanner.ConsumeNext(TokenKind.LeftSquareBrack);

                return new FuncNode(idToken.Value, Params().ToArray());

            }

            throw new UnexpectedTokenException("Expecting a function keyword")
            {
                UnexpectedToken = funcToken,
                ExpectedTokenType = typeof(IValueToken<Keyword>)
            };
        }

        private IEnumerable<ISymbolNode<TwisterPrimitive>> Params()
        {

            return null;
        }

        private INode Assignment(IExpressionNode<TwisterPrimitive> expression)
        {

            return null;
        }
    }
}
