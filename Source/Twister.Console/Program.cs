using System;
using Twister.Console;

namespace TwisterConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            CompileResult result = null;

            try
            {
                ParseArgs(args);

                var runner = new CompileRunner();

                result = runner.Compile();
                if (result == null)
                    throw new Exception("Unknown compile error");
            }
            catch (CommandLineArgumentException clae)
            {
                Console.WriteLine($"Invalid command: {clae.Argument}{(string.IsNullOrEmpty(clae.Suggestion) ? string.Empty : clae.Suggestion)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compile Error{Environment.NewLine}{ex}");
            }

            Console.WriteLine($"Compile completed successfully in {result.Duration:g}.");
        }

        private static void ParseArgs(string[] args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    default:
                        throw new CommandLineArgumentException("Invalid command option")
                        {
                            Argument = arg,
                            Suggestion = GetSuggestionIfAvailable(arg)
                        };
                }
            }
        }

        private static string GetSuggestionIfAvailable(string invalidArg)
        {
            return string.Empty;
        }
    }
}
