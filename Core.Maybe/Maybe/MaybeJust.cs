using System;
using System.Runtime.CompilerServices;

namespace Core.Maybe
{
  public static class MaybeJust
  {
    public static Maybe<T> Just<T>(this T? value, [CallerArgumentExpression("value")] string valueOrigin = "")
      where T : struct
    {
      if (value.HasValue)
      {
        return value.Value.ToMaybe();
      }

      throw new ArgumentNullException(nameof(value),
        $"Cannot create a Just<{typeof(T)}>, because {ValueOrigin(valueOrigin, nameof(value))} is null");
    }

    public static Maybe<T> Just<T>(this T? value, [CallerArgumentExpression("value")] string valueOrigin = "") where T : notnull
    {
      if (value != null)
      {
        return value.ToMaybe();
      }
      throw new ArgumentNullException(nameof(value), 
        $"Cannot create a Just<{typeof(T)}>, because {ValueOrigin(valueOrigin, nameof(value))} is null");
    }

    private static string ValueOrigin(string valueOrigin, string @default)
    {
        return valueOrigin != "" ? $"expression {{{valueOrigin}}}" : @default;
    }
  }
}
