using System;

/// <summary>
/// Has the task to locate a registered instance of the given type.
/// </summary>
public interface IServiceLocator
{
    /// <summary>
    /// Resolves an instance of the given type.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="T">The type of the wanted object.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    /// <returns>An instance of the given type.</returns>
    T Resolve<T>(string key = null) where T : class;

    /// <summary>
    /// Resolves an instance of the given type.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <param name="type">The type of the wanted object.</param>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    /// <returns>An instance of the given type.</returns>
    object Resolve(Type type, string key = null);
}