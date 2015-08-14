/// <summary>
/// Contains information of registered interfaces and their implementations
/// </summary>
public interface IIoCContainer
{
    /// <summary>
    /// Adds the given type to the containers registry.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    void Register<T>() where T : class;

    /// <summary>
    /// Adds the given interface to the containers registry and links it with an implementation of this interface.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    void Register<TInterface, TClass>(string key = null) where TClass : class, TInterface;

    /// <summary>
    /// Adds the given type to the containers registry. The instance of this type will be handled as a singleton.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    void RegisterSingleton<T>() where T : class;

    /// <summary>
    /// Adds the given instance to the containers registry. This instance will be handled as a singleton.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    /// <param name="instance">The instance that is going to be registered in the container.</param>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    void RegisterSingleton<T>(T instance, string key = null) where T : class;

    /// <summary>
    /// Adds the given interface to the containers registry and links it with an implementation of this interface.
    /// The instance of this type will be handled as a singleton.
    /// Multiple implementations of the same type are distinguished through a given key. 
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    void RegisterSingleton<TInterface, TClass>(string key = null) where TClass : class, TInterface;
}