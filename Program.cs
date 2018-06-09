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

            // Try to process the source code files
            try
            {
                ProcessFiles(args);
            }
            // If anything goes wrong print the error
            catch (FatalException e)
            {
                Console.Error.WriteLine("FATAL ERROR: " + e.Message);
                Environment.Exit(-1);
            }
        }

        private static void ProcessFiles(string[] filePaths)
        {
            // Load all the source files
            List<string> sourceFiles = new List<string>();

            foreach (string filePath in filePaths)
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
                    throw new FatalException($"Source file \"{filePath}\" does not exist");
                }
            }

            // Tokenize all the source code
            IEnumerable<Token> tokens = sourceFiles.SelectMany(x => Lexer.Tokenize(x));

            // No tokens
            // All source files are empty
            if (tokens.Count() == 0)
            {
                throw new FatalException("No source code provided (all source files are empty)");
            }

            // Parse the AST
            AbstractSyntaxTree ast = Parser.ParseAST(tokens);

            // Execute the program based on the parsed AST
            Interpreter inter = new Interpreter(ast);
            inter.Run();
        }
    }
}
