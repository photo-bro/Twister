using System.Collections.Generic;
using System.IO;

namespace Twister.Test.Data
{
    public static class TestProgramLoader
    {
        public const string ProgramDirectory = @"../../../Data/Test Programs/";

        public static string HelloWorld => File.ReadAllText(ProgramDirectory + @"HelloWorld.twt");

        public static string BasicArithmetic => File.ReadAllText(ProgramDirectory + @"BasicArithmetic.twt");

        public static string FizzBuzz => File.ReadAllText(ProgramDirectory + @"FizzBuzz.twt");

        public static string Literals => File.ReadAllText(ProgramDirectory + @"Literals.twt");

        public static IEnumerable<string> AllPrograms()
        {
            foreach (var file in Directory.GetFiles(ProgramDirectory, "*.twt"))
                yield return File.ReadAllText(file);
        }
    }
}
