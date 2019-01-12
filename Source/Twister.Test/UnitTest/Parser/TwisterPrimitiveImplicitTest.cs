using Xunit;
using Twister.Compiler.Parser.Primitive;

namespace Twister.Test.UnitTest.Parser
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity
    public class TwisterPrimitiveImplicitTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BoolPrimitive_To_Bool(bool expected)
        {
            var actual = (bool)(TwisterPrimitive)expected;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void IntPrimitive_To_Bool(int i, bool expected)
        {
            var actual = (bool)(TwisterPrimitive)i;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1u, true)]
        [InlineData(0u, false)]
        public void UIntPrimitive_To_Bool(uint u, bool expected)
        {
            var actual = (bool)(TwisterPrimitive)u;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1.11111d, true)]
        [InlineData(0.000001d, true)]
        [InlineData(0.0d, false)]
        [InlineData(-1.111d, false)]
        public void FloatPrimitive_To_Bool(double d, bool expected)
        {
            var actual = (bool)(TwisterPrimitive)d;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData('a', true)]
        [InlineData((char)0, false)]
        public void CharPrimitive_To_Bool(int c, bool expected)
        {
            var actual = (bool)(TwisterPrimitive)c;
            Assert.Equal(expected, actual);
        }
    }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
