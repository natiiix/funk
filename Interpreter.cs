using System;
using System.Linq;
using System.Collections.Generic;
using Funk.Expressions;

namespace Funk
{
    public class Interpreter
    {
        private readonly AbstractSyntaxTree ast;
        private readonly InterpreterEnvironment rootEnv;

        public Interpreter(AbstractSyntaxTree ast)
        {
            this.ast = ast;
            rootEnv = new InterpreterEnvironment();

            // Register built-in functions as symbols in the root environment
            DefineBuiltInFunctions();
        }

        public void Run()
        {
            foreach (IExpression expr in ast.TopLevelExpressions)
            {
                // Evaluate the top-level expressions
                IExpression value = expr.Evaluate(rootEnv);

                // Only print number expression results
                // If a top-level expression evaluates to anything else,
                // it was probably not intended to be printed
                // If the user wants to print the result anyways,
                // they can use the built-in print function
                if (value is NumberExpression)
                {
                    PrintExpression(value);
                }
            }
        }

        private void PrintExpression(IExpression expr)
        {
            // Empty expression:
            // Print nothing
            if (expr is VoidExpression)
            {
                return;
            }
            // Number expression:
            // Print just the numeric value
            else if (expr is NumberExpression)
            {
                Console.WriteLine((expr as NumberExpression).Value);
            }
            // Any other expression type:
            // Print the full string representation of the expression
            else
            {
                Console.WriteLine(expr);
            }
        }

        private void DefineBuiltInFunctions()
        {
            #region add
            rootEnv.Symbols["add"] = new BuiltInFunction((env, args) =>
            {
                // Check if all arguments are number expressions
                if (args.All(x => x is NumberExpression))
                {
                    return new NumberExpression(args.Sum(x => (x as NumberExpression).Value));
                }

                throw new UnexpectedArgumentTypeException("add");
            });
            #endregion

            #region sub
            rootEnv.Symbols["sub"] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // Insufficient number of arguments
                if (argCount < 1)
                {
                    throw new UnexpectedNumberOfArgumentsException("sub", ">= 1", argCount);
                }

                // Check if all arguments are number expressions
                if (args.All(x => x is NumberExpression))
                {
                    IEnumerable<int> values = args.Select(x => (x as NumberExpression).Value);
                    return new NumberExpression(values.First() - values.Skip(1).Sum());
                }

                throw new UnexpectedArgumentTypeException("sub");
            });
            #endregion

            #region mul
            rootEnv.Symbols["mul"] = new BuiltInFunction((env, args) =>
            {
                // Check if all arguments are number expressions
                if (args.All(x => x is NumberExpression))
                {
                    IEnumerable<int> values = args.Select(x => (x as NumberExpression).Value);

                    int result = 1;

                    foreach (int val in values)
                    {
                        result *= val;
                    }

                    return new NumberExpression(result);
                }

                throw new UnexpectedArgumentTypeException("mul");
            });
            #endregion

            #region div
            rootEnv.Symbols["div"] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // Insufficient number of arguments
                if (argCount < 1)
                {
                    throw new UnexpectedNumberOfArgumentsException("div", ">= 1", argCount);
                }

                // Check if all arguments are number expressions
                if (args.All(x => x is NumberExpression))
                {
                    IEnumerable<int> numExprs = args.Select(x => (x as NumberExpression).Value);

                    int result = numExprs.First();

                    foreach (int val in numExprs.Skip(1))
                    {
                        if (val == 0)
                        {
                            throw new FatalException("Unable to perform division by zero");
                        }

                        result /= val;
                    }

                    return new NumberExpression(result);
                }

                throw new UnexpectedArgumentTypeException("div");
            });
            #endregion

            #region mod
            rootEnv.Symbols["mod"] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // Insufficient number of arguments
                if (argCount < 1)
                {
                    throw new UnexpectedNumberOfArgumentsException("mod", ">= 1", argCount);
                }

                // Check if all arguments are number expressions
                if (args.All(x => x is NumberExpression))
                {
                    IEnumerable<int> numExprs = args.Select(x => (x as NumberExpression).Value);

                    int result = numExprs.First();

                    foreach (int val in numExprs.Skip(1))
                    {
                        if (val == 0)
                        {
                            throw new FatalException("Unable to calculate the remainder after division by zero");
                        }

                        result %= val;
                    }

                    return new NumberExpression(result);
                }

                throw new UnexpectedArgumentTypeException("mod");
            });
            #endregion

            #region print
            rootEnv.Symbols["print"] = new BuiltInFunction((env, args) =>
            {
                foreach (IExpression expr in args)
                {
                    PrintExpression(expr);
                }

                return new VoidExpression();
            });
            #endregion

            #region if
            rootEnv.Symbols["if"] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // Iterate through conditions
                for (int i = 0, nConditions = argCount / 2; i < nConditions; i++)
                {
                    // Get base index for this condition
                    int baseIdx = i * 2;

                    // Try to convert the condition expression to a numeric value
                    NumberExpression condition = args.ElementAt(baseIdx) as NumberExpression;

                    // Unable to convert the condition to a number expression
                    if (condition == null)
                    {
                        throw new UnexpectedArgumentTypeException("if");
                    }

                    // If the conditional evaluates to a truthy numeric value
                    if (condition.Value != 0)
                    {
                        // Return the expression supplied as the next argument
                        return args.ElementAt(baseIdx + 1);
                    }
                }

                // Final else statement
                if (argCount % 2 == 1)
                {
                    return args.ElementAt(argCount - 1);
                }
                // No condition met
                else
                {
                    return new VoidExpression();
                }
            });
            #endregion
        }
    }
}
