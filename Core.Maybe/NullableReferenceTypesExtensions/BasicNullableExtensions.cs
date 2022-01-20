using System;
using System.Threading.Tasks;

namespace Core.NullableReferenceTypesExtensions;

public static class BasicNullableExtensions
{
    public static T OrThrow<T>(this T? instance)
    {
        return instance.OrThrow(nameof(instance));
    }

    public static T OrThrow<T>(this T? instance, string instanceName)
    {
        return instance ??
               throw new InvalidOperationException(
                   $"Could not convert {instanceName} of type {typeof(T)}? " +
                   "to non-nullable reference type because it is null");
    }

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance)
    {
        return (await instance).OrThrow();
    }

    public static async Task<T> OrThrowAsync<T>(this Task<T?> instance, string instanceName)
    {
        return (await instance).OrThrow(instanceName);
    }
}