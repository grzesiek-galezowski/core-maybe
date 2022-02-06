using System;

namespace Core.Maybe
{
  public static class MaybeJust
  {
    public static Maybe<T> Just<T>(this T? value) where T : struct
    {
      if (value.HasValue)
      {
          return value.Value.ToMaybe();
      }

      throw new ArgumentNullException(nameof(value), "Cannot create a Just<" + typeof(T) + "> from null");
    }

    public static Maybe<T> Just<T>(this T? value) where T : notnull
    {
      if (value != null)
      {
        return value.ToMaybe();
      }
      throw new ArgumentNullException(nameof(value), "Cannot create a Just<" + typeof(T) + "> from null");
    }
  }
}
