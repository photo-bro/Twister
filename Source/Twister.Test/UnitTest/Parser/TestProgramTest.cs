using System;
using Twister.Test.Data;
using Xunit;

namespace Twister.Test.UnitTest.Parser
{
    public class TestProgramTest
    {
#pragma warning disable CS1701 // Assuming assembly reference matches identity

        [Fact(Skip = "Logic not implemented yet")]
        public void No_Exception_All()
        {
            var parser = TestUtility.CreateParser();
            foreach (var program in TestProgramLoader.AllPrograms())
            {
                var tokens = program.Item2.Tokenize();
                try
                {
                    var expected = parser.ParseProgram(tokens);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed for {program.Item1}:{Environment.NewLine}{program.Item2}", e);
                }
            }
        }
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    }
}
