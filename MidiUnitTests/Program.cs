using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiUnitTests
{
    class Program
    {
        /// <summary>
        /// Simple runner for the NUnit-like unit tests.
        /// </summary>
        /// <remarks>
        /// This program runs all of the tests defined in this assembly, using the small
        /// NUnit framework defined in Testing.cs.  When the MidiUnitTests project is
        /// Set As StartUp Project, pressing F5 builds the library and runs all unit tests,
        /// which is a convenient way to ensure that tests are run regularly.
        /// </remarks>
        static void Main(string[] args)
        {
            TestRunner.RunTestsInAssembly(typeof(Program).Assembly);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
