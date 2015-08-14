using System;

/// <summary>
/// Has the task to provide dependencies of the given object.
/// </summary>
public interface IDependencyInjector
{
    /// <summary>
    /// Injects public properties or fields that are marked with the Dependency attribute with the registered implementation.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    object Inject(Type type, object obj);

    /// <summary>
    /// Injects public properties or fields that are marked with the Dependency attribute with the registered implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    T Inject<T>(object obj);
}