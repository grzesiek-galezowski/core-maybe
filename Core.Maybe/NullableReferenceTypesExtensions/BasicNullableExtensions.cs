using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.NullableReferenceTypesExtensions;

public static class BasicNullableExtensions
{
  public static T OrThrow<T>(
    this T? instance, [CallerArgumentExpression(nameof(instance))] string callerArg = "") =>
    instance ??
    throw new InvalidOperationException(
      $"Could not convert the result of {{{callerArg}}} of type {typeof(T)}? " +
      "to non-nullable reference type because it is null");

  public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, [CallerArgumentExpression(nameof(instance))] string callerArg = "")
    => (await instance).OrThrow(callerArg);

  public static T OrThrow<T>(
    this T? instance,
    [CallerArgumentExpression(nameof(instance))] string callerArg = "") 
    where T : struct
  {
    if (instance.HasValue)
    {
      return instance.Value;
    }
    else
    {
      throw new InvalidOperationException($"Could not convert the result of {{{callerArg}}} of type {typeof(T)}? " +
                                          "to non-nullable struct because it is null");
    }
  }

  public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, [CallerArgumentExpression(nameof(instance))] string callerArg = "") where T : struct
    => (await instance).OrThrow(callerArg);
}