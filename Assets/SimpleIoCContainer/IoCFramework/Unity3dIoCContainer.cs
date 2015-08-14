using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
public class Unity3dIoCContainer : IIoCContainer, IServiceLocator, IDependencyInjector
{
    private class TypeData
    {
        public object Instance { get; set; }
       
        public List<KeyValuePair<DependencyAttribute, PropertyInfo>> Properties { get; private set; }

        public List<KeyValuePair<DependencyAttribute, FieldInfo>> Fields { get; private set; }

        public bool IsSingleton { get; private set; }

        private TypeData()
        {
            this.Properties = new List<KeyValuePair<DependencyAttribute, PropertyInfo>>();
            this.Fields = new List<KeyValuePair<DependencyAttribute, FieldInfo>>();
        }

        public static TypeData Create(Type type, bool isSingleton = false, object instance = null)
        {
            var typeData = new TypeData { IsSingleton = isSingleton, Instance = instance };

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var dependency =
                    (DependencyAttribute)field.GetCustomAttributes(typeof(DependencyAttribute), true).FirstOrDefault();
                if (dependency == null) continue;

                typeData.Fields.Add(new KeyValuePair<DependencyAttribute, FieldInfo>(dependency, field));
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var dependency =
                    (DependencyAttribute)property.GetCustomAttributes(typeof(DependencyAttribute), true).FirstOrDefault();
                if (dependency == null) continue;

                typeData.Properties.Add(new KeyValuePair<DependencyAttribute, PropertyInfo>(dependency, property));
            }

            return typeData;
        }
    }

    /// <summary>
    /// The name of the game object, that will be used to attach singleton monobehaviours.
    /// </summary>
    private string singletonGameObjectName;

    /// <summary>
    /// Holds all information of the registered types.
    /// </summary>
    private readonly Dictionary<Type, Dictionary<string, Type>> types = new Dictionary<Type, Dictionary<string, Type>>();

    /// <summary>
    /// The type containers
    /// </summary>
    private readonly Dictionary<Type, TypeData> typeDatas = new Dictionary<Type, TypeData>();

    public Unity3dIoCContainer()
    {
        this.RegisterSingleton<IServiceLocator>(this);
    }

    #region IIoCContainer Members

    /// <summary>
    /// Adds the given type to the containers registry.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    public void Register<T>() where T : class
    {
        var type = typeof(T);
        var typeData = TypeData.Create(type);
        this.Register(null, type, typeData);
    }

    /// <summary>
    /// Adds the given interface to the containers registry and links it with an implementation of this interface.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    public void Register<TInterface, TClass>(string key = null) where TClass : class, TInterface
    {
        var typeInterface = typeof(TInterface);
        var type = typeof(TClass);
        var typeData = TypeData.Create(type);
        this.Register(typeInterface, type, typeData, key);
    }

    /// <summary>
    /// Adds the given type to the containers registry. The instance of this type will be handled as a singleton.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    public void RegisterSingleton<T>() where T : class
    {
        var type = typeof(T);
        var typeData = TypeData.Create(type, true);
        this.Register(null, type, typeData);
    }

    /// <summary>
    /// Adds the given instance to the containers registry. This instance will be handled as a singleton.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    /// <param name="instance">The instance that is going to be registered in the container.</param>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    public void RegisterSingleton<T>(T instance, string key = null) where T : class
    {
        var type = typeof(T);
        var typeData = TypeData.Create(type, true, instance);
        this.Register(null, type, typeData, key);
    }

    /// <summary>
    /// Adds the given interface to the containers registry and links it with an implementation of this interface.
    /// The instance of this type will be handled as a singleton.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    public void RegisterSingleton<TInterface, TClass>(string key = null) where TClass : class, TInterface
    {
        var typeInterface = typeof (TInterface);
        var type = typeof (TClass);
        var typeData = TypeData.Create(type, true);
        this.Register(typeInterface, type, typeData, key);
    }

    /// <summary>
    /// Resolves an instance of the given type.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <typeparam name="T">The type of the wanted object.</typeparam>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    /// <returns>
    /// An instance of the given type.
    /// </returns>
    public T Resolve<T>(string key = null) where T : class
    {
        return (T)this.Resolve(typeof(T), key);
    }

    /// <summary>
    /// Injects public properties or fields that are marked with the Dependency attribute with the registered implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public T Inject<T>(object obj)
    {
        return (T)this.Inject(typeof(T), obj);
    }

    /// <summary>
    /// Injects public properties or fields that are marked with the Dependency attribute with the registered implementation.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public object Inject(Type type, object obj)
    {
        var typeData = this.GetTypeData(type);

        typeData.Fields.ForEach(x => x.Value.SetValue(obj, this.Resolve(x.Value.FieldType, x.Key.Key)));
        typeData.Properties.ForEach(x => x.Value.SetValue(obj, this.Resolve(x.Value.PropertyType, x.Key.Key), null));

        return obj;
    }

    private TypeData GetTypeData(Type type)
    {
        if (!this.typeDatas.ContainsKey(type))
        {
             var typeData = TypeData.Create(type);
             this.Register(null, type, typeData);
        }

        return this.typeDatas[type];
    }

    /// <summary>
    /// Resolves an instance of the given type.
    /// Multiple implementations of the same type are distinguished through a given key.
    /// </summary>
    /// <param name="type">The type of the wanted object.</param>
    /// <param name="key">Optional.The key to distinguish between implementations of the same interface.</param>
    /// <returns>
    /// An instance of the given type.
    /// </returns>
    public object Resolve(Type type, string key = null)
    {
        Guard(!this.types.ContainsKey(type), "The type {0} is not registered.", type.Name);

        Guard(!this.types[type].ContainsKey(key ?? string.Empty),
            "There is no implementation registered with the key {0} for the type {1}.", key, type.Name);

        var foundType = this.types[type][key ?? string.Empty];

        var typeData = this.typeDatas[foundType];

        if (foundType.IsSubclassOf(typeof(MonoBehaviour))) // this is the unity3d specific part
        {
            Guard(this.singletonGameObjectName == null,
                "You have to set a game object name to use for MonoBehaviours with SetSingletonGameObject() first.");

            // places a new empty game object in the game if not found
            var gameObject = GameObject.Find(this.singletonGameObjectName)
                ?? new GameObject(this.singletonGameObjectName);

            // when the game already has the wanted component attached, return it.
            // if thats not the case add the component to the object and inject possible dependecies.
            return gameObject.GetComponent(type.Name) ?? Inject(foundType, gameObject.AddComponent(foundType));
        }

        if (typeData.IsSingleton)
        {
            // if an instance already exists, return it.
            // if thats not the case setup a new instance and inject all dependencies.
            return typeData.Instance ?? (typeData.Instance = this.Setup(foundType));
        }

        return this.Setup(foundType);
    }

    #endregion

    /// <summary>
    /// Sets the name of the game object that will be used to attach components.
    /// </summary>
    /// <param name="name">The name.</param>
    public void SetSingletonGameObject(string name)
    {
        this.singletonGameObjectName = name;
    }

    private object Setup(Type type)
    {
        var instance = Activator.CreateInstance(type);
        this.Inject(type, instance);
        return instance;
    }

    private static void Guard(bool failed, string format, params object[] args)
    {
        if (failed) throw new IoCContainerException(string.Format(format, args), null);
    }

    private void Register(Type interfaceType, Type type, TypeData typeData, string key = null)
    {
        try
        {
            if (this.types.ContainsKey(interfaceType ?? type))
            {
                this.types[interfaceType ?? type].Add(key ?? string.Empty, type);
            }
            else
            {
                this.types.Add(interfaceType ?? type, new Dictionary<string, Type> { { key ?? string.Empty, type } });
            }

            this.typeDatas.Add(type, typeData);
        }
        catch (Exception ex)
        {
            throw new IoCContainerException("Register type failed.", ex);
        }
    }
}
