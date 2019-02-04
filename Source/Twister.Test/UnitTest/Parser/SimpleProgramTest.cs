using Twister.Compiler.Parser.Interface;
using Twister.Compiler.Parser.Node;
using Xunit;
using InvalidProgramException = Twister.Compiler.Parser.InvalidProgramException;

namespace Twister.Test.UnitTest.Parser
{
#pragma warning disable CS1701 // Assuming assembly reference matches identity

    public class SimpleProgramTest
    {
        [Fact]
        public void EmptyProgram()
        {
            // Arrange
            var program = "";
            var programTokens = program.Tokenize();
            var parser = TestUtility.CreateParser();
            var expected = new ProgramNode(new INode[] { });

            // Act & Assert
            ProgramNode actual;
            Assert.Throws<InvalidProgramException>(() => actual = parser.ParseProgram(programTokens) as ProgramNode);
        }


        [Fact(Skip = "Not implemented yet")]
        public void EmptyMain()
        {
            // Arrange
            var program = "func main : { }";
            var programTokens = program.Tokenize();
            var parser = TestUtility.CreateParser();
            var expected = new ProgramNode(new INode[] { });
            INode actual;

            // Act 
            actual = parser.ParseProgram(programTokens);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<ProgramNode>(actual);

            var actualProgramNode = actual as ProgramNode;
            Assert.Equal(actualProgramNode, expected);

            // TODO build expected AST
        }

        [Fact]
        public void EmptyBody()
        {

        }
    }

#pragma warning restore CS1701 // Assuming assembly reference matches identity
}
