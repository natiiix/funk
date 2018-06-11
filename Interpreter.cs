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

            #region batch
            rootEnv.Symbols["batch"] = new BuiltInFunction((env, args) =>
            {
                // Return the last evaluated expression passed to the function
                // or void expression is no argument was provided
                return args.LastOrDefault() ?? new VoidExpression();
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
                    if (condition.BooleanValue)
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

            #region not
            rootEnv.Symbols["not"] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // The function must be applied to a single number expression
                if (argCount != 1)
                {
                    throw new UnexpectedNumberOfArgumentsException("not", 1, argCount);
                }

                NumberExpression numExpr = args.ElementAt(0) as NumberExpression;

                // Invalid expression type
                if (numExpr == null)
                {
                    throw new UnexpectedArgumentTypeException("not");
                }

                // Invert the boolean value of the number expression and return it
                return new NumberExpression(!numExpr.BooleanValue);
            });
            #endregion

            // Comparison functions are always performed on two consecutive expressions
            // This can yield unexpected results (e.g., when calling not_equal with more than two arguments)
            // Example: (not_equal 1 0 1) evaluates to true because no two consecutive expressions are equal
            AddComparisonFunction("equal", (a, b) => a == b);
            AddComparisonFunction("not_equal", (a, b) => a != b);
            AddComparisonFunction("greater", (a, b) => a > b);
            AddComparisonFunction("greater_or_equal", (a, b) => a >= b);
            AddComparisonFunction("less", (a, b) => a < b);
            AddComparisonFunction("less_or_equal", (a, b) => a <= b);

            // Logical boolean functions
            AddBooleanFunction("and", (nTrue, nTotal) => nTrue == nTotal);
            AddBooleanFunction("or", (nTrue, nTotal) => nTrue > 0);
            AddBooleanFunction("xor", (nTrue, nTotal) => nTrue == 1);
        }

        private delegate bool ComparisonFunction(int first, int second);

        private void AddComparisonFunction(string name, ComparisonFunction compFunc)
        {
            // Register the built-in function as a symbol in the root environment
            rootEnv.Symbols[name] = new BuiltInFunction((env, args) =>
            {
                int argCount = args.Count();

                // Comparison functions require at least two arguments
                if (argCount < 2)
                {
                    throw new UnexpectedNumberOfArgumentsException(name, ">= 2", argCount);
                }

                // Convert all arguments to number expression
                IEnumerable<NumberExpression> numArgs = args.Select(x => x as NumberExpression);

                // If any of the arguments cannot be converted to a number
                if (numArgs.Any(x => x == null))
                {
                    throw new UnexpectedArgumentTypeException(name);
                }

                // Convert the number expressions to integers
                IEnumerable<int> intArgs = numArgs.Select(x => x.Value);

                // Get the first expression
                int previous = intArgs.First();

                // Check if the comparison function applies to all of the values
                // Every value is compared to the value before it
                foreach (int val in intArgs.Skip(1))
                {
                    if (compFunc(previous, val))
                    {
                        previous = val;
                    }
                    else
                    {
                        return new NumberExpression(false);
                    }
                }

                // All of the values have passed the comparison test
                return new NumberExpression(true);
            });
        }

        private delegate bool BooleanEvaluationFunction(int numberOfTrue, int totalNumberOfArgs);

        private void AddBooleanFunction(string name, BooleanEvaluationFunction boolEval)
        {
            // Register the built-in function as a symbol in the root environment
            rootEnv.Symbols[name] = new BuiltInFunction((env, args) =>
            {
                // Convert all arguments to number expression
                IEnumerable<NumberExpression> numArgs = args.Select(x => x as NumberExpression);

                // If any of the arguments cannot be converted to a number expression
                if (numArgs.Any(x => x == null))
                {
                    throw new UnexpectedArgumentTypeException(name);
                }

                // Count the number expressions that evaluate to true
                int trueCount = numArgs.Count(x => x.BooleanValue);

                // Evaluate the number of true values
                bool result = boolEval(trueCount, args.Count());

                return new NumberExpression(result);
            });
        }
    }
}
