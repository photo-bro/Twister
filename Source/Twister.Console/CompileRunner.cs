using System;
using System.Diagnostics;

namespace Twister.Console
{
    public class CompileRunner
    {
        public CompileResult Compile()
        {
            var sw = Stopwatch.StartNew();

            //TODO

            sw.Stop();

            return new CompileResult
            {
                Duration = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
            };
        }
    }
}
