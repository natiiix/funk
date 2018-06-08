# Funk Language Specification (early draft)

Heavily inspired by Lisp.
May be subject to change in the future.

## Function Definition

```
(function <FunctionName> ([Parameters]) <Expression>)
```

## Expression / Function Call

```
(<FunctionName> [Parameters])
```

# Syntax Explanation

```
1234
```
The literal numeric value `1234`.

```
a
```
The value of `a`.

```
(a)
```
Call function `a`.

```
(a b c)
```
Call function `a` with parameters `b` and `c`.

```
(function a (b c) (b c))
```
Define function `a` with parameters `b` and `c`.
This function calls the function `b` (which has been passed as a parameter to `a`) with `c` as a parameter.
Calling `(a b c)` yields the same result as calling `(b c)` in this scenario.
