# Funk Language Specification (early draft)

Heavily inspired by Lisp.
May be subject to change in the future.

## Function Definition

```funk
(function <FunctionName> ([Parameters]) <Expression>)
```

## Expression / Function Call

```funk
(<FunctionName> [Parameters])
```

## Syntax Explanation

```funk
1234
```

The literal numeric value `1234`.

```funk
a
```

The value of `a`.

```funk
(a)
```

Call function `a`.

```funk
(a b c)
```

Call function `a` with parameters `b` and `c`.

```funk
(function a (b c) (b c))
```

Define function `a` with parameters `b` and `c`.
This function calls the function `b` (which has been passed as a parameter to `a`) with `c` as a parameter.
Calling `(a b c)` yields the same result as calling `(b c)` in this scenario.

## Boolean Values

To make things simples, boolean values have been omitted from the language.
They may be added in the future, but right now there is no need for them.
For all intents and purposes, the system used by ANSI C has been adapted, which means that `0` is treater as false and every other numeric value (including negative numeric values) is treated as true.

## Built-in Functions

To make the built-in functions easier to understand for newcomers, this paragraph will contain their transcription to the C# language.

### add

Description: Numeric addition

Funk syntax:

```funk
(add a b c d)
```

C# syntax:

```csharp
a + b + c + d
```

Expected number of parameters: 0 or more

### sub

Description: Numeric subtraction

Funk syntax:

```funk
(sub a b c d)
```

C# syntax:

```csharp
a - b - c - d
```

Expected number of parameters: 1 or more

### mul

Description: Numeric multiplication

Funk syntax:

```funk
(mul a b c d)
```

C# syntax:

```csharp
a * b * c * d
```

Expected number of parameters: 0 or more

### div

Description: Numeric division

Funk syntax:

```funk
(div a b c d)
```

C# syntax:

```csharp
a / b / c / d
```

Expected number of parameters: 1 or more

### mod

Description: Remainder after integer division

Funk syntax:

```funk
(mod a b c d)
```

C# syntax:

```csharp
a % b % c % d
```

Expected number of parameters: 1 or more

### print

Description: Prints the values of an arbitrary number of expressions passed to the function. The value of each expression is printed on a separate line.

Funk syntax:

```funk
(print a b c d)
```

C# syntax:

```csharp
Console.WriteLine(a);
Console.WriteLine(b);
Console.WriteLine(c);
Console.WriteLine(d);
```

Expected number of parameters: 0 or more

### batch

Description: Evaluates a batch of expressions and returns the result of evaluating the last of them. If no parameters are passed to the function, void expression is returned.

Funk syntax:

```funk
(batch a b c d)
```

C# syntax: \*

```csharp
Expression batch(Expression a, Expression b, Expression c, Expression d)
{
    a.Evaluate();
    b.Evaluate();
    c.Evaluate();
    return d.Evaluate();
}
```

Expected number of parameters: 0 or more

### if

Description: Standard `if` statement written in a functional form. The final `else` statement is optional. Void expression is returned if none of the conditions are met and the final else statement is omitted.

Funk syntax:

```funk
(if a b c d e)
```

C# syntax: \*

```csharp
Expression if(Expression a, Expression b, Expression c, Expression d, Expression e)
{
    if (a)
    {
        return b.Evaluate();
    }
    else if (c)
    {
        return d.Evaluate();
    }
    else
    {
        return e.Evaluate();
    }
}
```

Expected number of parameters: 0 or more

### not

Description: Boolean NOT operation

Funk syntax:

```funk
(not a)
```

C# syntax:

```csharp
a != 0
```

Expected number of parameters: 1

### Numeric Comparison Functions

- `equal` - Determines if all of the arguments are equal
- `not_equal` - Determines if every two consecutive argument are not equal
- `greater` - Determines if every argument is greater than the following one
- `greater_or_equal` - Determines if every argument is greater or equal to the following one
- `less` - Determines if every argument is lower than the following one
- `less_or_equal` - Determines if every argument is lower or equal than the following one

### Logical Boolean Functions

- `and` - Determines if all of the arguments are non-zero numeric expressions
- `or` - Determines if at least one of the arguments is a non-zero numeric expression
- `xor` - Determines if exactly one of the arguments is a non-zero numeric expression

### Note

\* There is no good way to transcribe this function into C# due to the differences between Funk and C# in terms of language design. Instead, a simplified implementation of the function is provided.
