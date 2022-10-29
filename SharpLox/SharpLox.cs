using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class SharpLox
    {
        public static bool ErrorOccured { get; set; }

        public void RunFile(string filePath)
        {
            RunCode(File.ReadAllText(filePath));
            if (ErrorOccured)
            {
                System.Environment.Exit(65);
            }
        }

        public void RunPrompt()
        {
            while(true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line is null) break;

                RunCode(line);
                ErrorOccured = false;
            }
        }

        private void RunCode(string code)
        {
            var scanner = new Scanner(code);
            var tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }

        public static void Error(int lineNumber, string message)
        {
            Report(lineNumber, "", message);
        }

        private static void Report(int lineNumber, string where, string message)
        {
            Console.WriteLine($"[{lineNumber}]: Error {where}: {message}");
            ErrorOccured = true;
        }
    }
}
