# Core.Maybe 8.0.0

## Highlights

- Split the old dual-purpose `Either<TResult, TError>` type into two distinct types:
  - `Either<TLeft, TRight>` for left/right-style branching
  - `Result<TValue, TError>` for success/error-style branching
- Added explicit factory methods and extension methods for both types:
  - `Either.Left(...)` / `Either.Right(...)`
  - `Result.Value(...)` / `Result.Error(...)`
  - `ToLeft(...)` / `ToRight(...)`
  - `ToValue(...)` / `ToError(...)`

## Breaking changes

The previous `Either<TResult, TError>` API combined both a left/right and success/error model. In 8.0.0 it has been replaced by two separate types, so code that relied on the old shape must be updated.

### Migration guide

#### 1. Replace `Either` usage that represented success/error with `Result`

Before:

```csharp
using Core.Either;

Either<int, string> value = 42;
Either<int, string> error = "boom";

var resultValue = value.ResultOrDefault();
var errorValue = error.ErrorOrDefault();
```

After:

```csharp
using Core.Either;

Result<int, string> value = 42;
Result<int, string> error = "boom";

var resultValue = value.ValueOrDefault();
var errorValue = error.ErrorOrDefault();
```

#### 2. Replace `Either` usage that represented left/right with `Either`

Before:

```csharp
using Core.Either;

Either<string, int> either = "left";
var left = either.LeftOrDefault();
var right = either.RightOrDefault();
```

After:

```csharp
using Core.Either;

Either<string, int> either = "left";
var left = either.LeftOrDefault();
var right = either.RightOrDefault();
```

The main difference is that the type name is now used consistently for left/right semantics, while success/error semantics now use `Result`.

#### 3. Update factory methods and extension methods

Before:

```csharp
var either = Either<string, int>.Result("ok");
var error = Either<string, int>.Error(42);
var left = 10.ToResult<int, string>();
```

After:

```csharp
var either = Either<string, int>.Left("ok");
var error = Result<string, int>.Error(42);
var left = 10.ToLeft<int, string>();
```

#### 4. Update matching calls

Before:

```csharp
either.Match(x => x, y => y.ToString());
```

After:

```csharp
either.Match(x => x, y => y.ToString());
```

For result-style code, use `Result.Match(...)` instead of relying on the old `Either` API.

## Notes

This release is a semantic-major bump because it introduces a breaking API change. Existing consumers should update their code to use `Either` for left/right scenarios and `Result` for success/error scenarios.
