using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Funk
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // No arguments provided
            // Print the expected syntax
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: funk <source file> [<source file> ...]");
                Environment.Exit(0);
            }

            // Load all the source files
            List<string> sourceFiles = new List<string>();

            foreach (string filePath in args)
            {
                // File exists
                // Read it
                if (File.Exists(filePath))
                {
                    sourceFiles.Add(File.ReadAllText(filePath));
                }
                // File does not exist
                // Print an error and stop execution
                else
                {
                    ExitWithError($"Source file \"{filePath}\" does not exist");
                }
            }

            // Tokenize all the source code
            IEnumerable<Token> tokens = sourceFiles.SelectMany(x => Lexer.Tokenize(x));

            // No tokens
            // All source files are empty
            if (tokens.Count() == 0)
            {
                ExitWithError("No source code provided (all source files are empty)");
            }

            // Parse the AST
            AbstractSyntaxTree ast = Parser.ParseAST(tokens);

            // Execute the program based on the parsed AST
            Interpreter inter = new Interpreter(ast);
            inter.Run();
        }

        public static void ExitWithError(string errorMessage, int errorCode = -1)
        {
            Console.Error.WriteLine("ERROR: " + errorMessage);
            Environment.Exit(-1);
        }
    }
}
