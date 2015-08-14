using System;

/// <summary>
/// Indicates that this member should be injected through the IoC container.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DependencyAttribute : Attribute
{
    /// <summary>
    /// The key to distinguish between implementations of the same interface.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Indicates that this member should be injected through the IoC container.
    /// </summary>
    public DependencyAttribute()
    {
    }

    /// <summary>
    /// Indicates that this member should be injected through the IoC container.
    /// </summary>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    public DependencyAttribute(string key)
    {
        this.Key = key;
    }
}
