using System.Threading.Tasks;

namespace Core.Maybe
{
  public static class MaybeJustAsync
  {
    public static async Task<Maybe<T>> JustAsync<T>(this Task<T?> value) where T : notnull
    {
      return (await value).Just();
    }

    public static async Task<Maybe<T>> JustAsync<T>(this Task<T?> value) where T : struct
    {
      return (await value).Just();
    }
      
    public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<T?> value) where T : notnull
    {
      return (await value).ToMaybe();
    }

    public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<T?> value) where T : struct
    {
      return (await value).ToMaybe();
    }
  }
}