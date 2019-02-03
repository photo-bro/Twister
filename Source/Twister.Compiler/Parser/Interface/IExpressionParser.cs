using System;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Compiler.Parser.Interface
{
    public interface IExpressionParser
    {
        IValueNode<TwisterPrimitive> ParseArithmeticExpression(ITokenMatcher matcher, IScopeManager scopeManager,
            Func<IValueNode<TwisterPrimitive>> assignmentCallback);
    }
}
