using System;
using System.Threading.Tasks;

namespace Core.Maybe
{
  public static class MaybeJustAsync
  {
    public static async Task<Maybe<T>> JustAsync<T>(this Task<T?> value) where T : notnull 
        => (await value).Just();

    public static async Task<Maybe<T>> JustAsync<T>(this Task<T?> value) where T : struct 
        => (await value).Just();

    public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<T?> value) where T : notnull 
        => (await value).ToMaybe();

    public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<T?> value) where T : struct 
        => (await value).ToMaybe();
  }
}