using Xunit;
using Twister.Compiler.Parser.Enum;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Test.UnitTest.Parser
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity
    public class TwisterPrimitiveOperatorTest
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void LogAnd(bool a, bool b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Bool) { Bool = a };
            var right = new TwisterPrimitive(PrimitiveType.Bool) { Bool = b };

            var actual = left && right;
            var expected = a && b;

            Assert.Equal(expected, actual.Bool);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void LogOr(bool a, bool b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Bool) { Bool = a };
            var right = new TwisterPrimitive(PrimitiveType.Bool) { Bool = b };

            var actual = left || right;
            var expected = a || b;

            Assert.Equal(expected, actual.Bool);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0XFF, 0)]
        [InlineData(0XFF, 0xFF)]
        public void BitAnd_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left & right;
            var expected = a & b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0u)]
        [InlineData(0XFF, 0u)]
        [InlineData(0XFF, 0xFFu)]
        public void BitAnd_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left & right;
            var expected = (uint)(a & b);

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0XFF, 0)]
        [InlineData(0XFF, 0xFF)]
        public void BitOr_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left | right;
            var expected = a | b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0u)]
        [InlineData(0XFF, 0u)]
        [InlineData(0XFF, 0xFFu)]
        public void BitOr_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left | right;
            var expected = (uint)(a | (int)b);

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 4)]
        [InlineData(-100, 25)]
        [InlineData(-100, -25)]
        public void Addition_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left + right;
            var expected = a + b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0u)]
        [InlineData(5, 10u)]
        [InlineData(-5, 10u)]
        public void Addition_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left + right;
            var expected = a + b;

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(0, 0d)]
        [InlineData(1, 9.99d)]
        [InlineData(1, -9.99d)]
        public void Addition_Int_Float(int a, double b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Float) { Float = b };

            var actual = left + right;
            var expected = a + b;

            Assert.Equal(expected, actual.Float);
        }

        [Theory]
        [InlineData(0, (char)0)]
        [InlineData(32, 'A')]
        [InlineData(1, 'A')]
        public void Addition_Int_Char(int a, char b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Char) { Char = b };

            var actual = left + right;
            var expected = a + b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 4)]
        [InlineData(-100, 25)]
        [InlineData(-100, -25)]
        public void Subtract_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left - right;
            var expected = a - b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0u)]
        [InlineData(15, 10u)]
        [InlineData(10, 10u)]
        public void Subtract_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left - right;
            var expected = a - b;

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(0, 0d)]
        [InlineData(1, 9.99d)]
        [InlineData(1, -9.99d)]
        public void Subtract_Int_Float(int a, double b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Float) { Float = b };

            var actual = left - right;
            var expected = a - b;

            Assert.Equal(expected, actual.Float);
        }

        [Theory]
        [InlineData(0, (char)0)]
        [InlineData(32, 'a')]
        [InlineData(1, 'B')]
        public void Subtract_Int_Char(int a, char b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Char) { Char = b };

            var actual = left - right;
            var expected = a - b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 4)]
        [InlineData(-100, 25)]
        [InlineData(-100, -25)]
        public void Mult_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left * right;
            var expected = a * b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(0, 0u)]
        [InlineData(15, 10u)]
        [InlineData(10, 10u)]
        public void Mult_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left * right;
            var expected = a * b;

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(0, 0d)]
        [InlineData(1, 9.99d)]
        [InlineData(1, -9.99d)]
        public void Mult_Int_Float(int a, double b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Float) { Float = b };

            var actual = left * right;
            var expected = a * b;

            Assert.Equal(expected, actual.Float);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(2, 1)]
        [InlineData(-2, 1)]
        public void Mult_Int_Char(int a, char b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Char) { Char = b };

            var actual = left * right;
            var expected = a * b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(-100, 25)]
        [InlineData(-100, -25)]
        [InlineData(12, 1)]
        public void Div_Int_Int(int a, int b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Int) { Int = b };

            var actual = left / right;
            var expected = a / b;

            Assert.Equal(expected, actual.Int);
        }

        [Theory]
        [InlineData(2, 2u)]
        [InlineData(2, 1u)]
        public void Div_Int_UInt(int a, uint b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.UInt) { UInt = b };

            var actual = left / right;
            var expected = a / b;

            Assert.Equal(expected, actual.UInt);
        }

        [Theory]
        [InlineData(1, 9.99d)]
        [InlineData(1, -9.99d)]
        [InlineData(10, 0.00001d)]
        public void Div_Int_Float(int a, double b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Float) { Float = b };

            var actual = left / right;
            var expected = a / b;

            Assert.Equal(expected, actual.Float);
        }

        [Theory]
        [InlineData(2, 'A')]
        [InlineData(1, 'B')]
        public void Div_Int_Char(int a, char b)
        {
            var left = new TwisterPrimitive(PrimitiveType.Int) { Int = a };
            var right = new TwisterPrimitive(PrimitiveType.Char) { Char = b };

            var actual = left / right;
            var expected = a / b;

            Assert.Equal(expected, actual.Int);
        }

    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
