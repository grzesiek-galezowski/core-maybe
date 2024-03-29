﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.NullableReferenceTypesExtensions;

public static class BasicNullableExtensions
{
    public static T OrThrow<T>(
      this T? instance, [CallerArgumentExpression("instance")] string callerArg = "") =>
      instance ??
      throw new InvalidOperationException(
        $"Could not convert the result of {{{callerArg}}} of type {typeof(T)}? " +
        "to non-nullable reference type because it is null");

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, [CallerArgumentExpression("instance")] string callerArg = "") 
      => (await instance).OrThrow(callerArg);

    public static T OrThrow<T>(this T? instance, [CallerArgumentExpression("instance")] string callerArg = "") where T : struct
    {
      try
      {
        return instance!.Value;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException($"Could not convert the result of {{{callerArg}}} of type {typeof(T)}? " +
                                            "to non-nullable struct because it is null", ex);
      }
    }

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, [CallerArgumentExpression("instance")] string callerArg = "") where T : struct
      => (await instance).OrThrow(callerArg);
}