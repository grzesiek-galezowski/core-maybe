using System;
using System.Threading.Tasks;

namespace Core.NullableReferenceTypesExtensions;

public static class BasicNullableExtensions
{
    public static T OrThrow<T>(this T? instance) => instance.OrThrow(nameof(instance));

    public static T OrThrow<T>(this T? instance, string instanceName) =>
      instance ??
      throw new InvalidOperationException(
        $"Could not convert {instanceName} of type {typeof(T)}? " +
        "to non-nullable reference type because it is null");

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance) 
      => (await instance).OrThrow();

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, string instanceName) 
      => (await instance).OrThrow(instanceName);

    public static T OrThrow<T>(this T? instance) where T : struct
      => instance!.Value;
    
    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance) where T : struct
      => (await instance).OrThrow();
}