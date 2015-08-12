using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

#if !ZEN_NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    // Note that probably want to use the generic versions of these methods in InstantiatorExtensions
    public interface IInstantiator
    {
        // For most cases you can pass in currentContext and concreteIdentifier as null
        object InstantiateExplicit(
            Type concreteType, List<TypeValuePair> extraArgMap, InjectContext currentContext, string concreteIdentifier, bool autoInject);

#if !ZEN_NOT_UNITY3D

        // Create a new game object from a given prefab
        // Without returning any particular monobehaviour
        // If you want to retrieve a specific monobehaviour use InstantiatePrefabForComponent
        GameObject InstantiatePrefabExplicit(
            GameObject prefab, IEnumerable<object> extraArgMap, InjectContext currentContext);

        // Instantiate the given prefab, inject on all MonoBehaviours, then return the instance of 'componentType'
        // Any arguments supplied are assumed to be used as extra parameters into 'componentType'
        object InstantiatePrefabForComponentExplicit(
            Type componentType, GameObject prefab, List<TypeValuePair> extraArgMap, InjectContext currentContext);

        // Instantiate an empty game object and then add a component to it of type 'componentType'
        object InstantiateComponentOnNewGameObjectExplicit(
            Type componentType, string name, List<TypeValuePair> extraArgMap, InjectContext currentContext);

        // Add a MonoBehaviour to an existing game object
        // NOTE: Gameobject here is not a prefab prototype, it is an instance
        Component InstantiateComponent(
            Type componentType, GameObject gameObject, params object[] extraArgMap);

        // Create a new empty game object under the root transform
        GameObject InstantiateGameObject(string name);
#endif
    }

    public static class InstantiatorExtensions
    {
#if !ZEN_NOT_UNITY3D

        public static TContract InstantiateComponent<TContract>(
            this IInstantiator container, GameObject gameObject, params object[] args)
            where TContract : Component
        {
            return (TContract)container.InstantiateComponent(typeof(TContract), gameObject, args);
        }

        public static GameObject InstantiatePrefab(
            this IInstantiator container, GameObject prefab, params object[] args)
        {
            return container.InstantiatePrefabExplicit(prefab, args, null);
        }

        /////////////// InstantiatePrefabForComponent

        public static T InstantiatePrefabForComponent<T>(
            this IInstantiator container, GameObject prefab, params object[] extraArgs)
        {
            return (T)container.InstantiatePrefabForComponent(typeof(T), prefab, extraArgs);
        }

        public static object InstantiatePrefabForComponent(
            this IInstantiator container, Type concreteType, GameObject prefab, params object[] extraArgs)
        {
            Assert.That(!extraArgs.Contains(null),
                "Null value given to factory constructor arguments when instantiating object with type '{0}'. In order to use null use InstantiatePrefabForComponentExplicit", concreteType);

            return container.InstantiatePrefabForComponentExplicit(
                concreteType, prefab, InstantiateUtil.CreateTypeValueList(extraArgs));
        }

        // This is used instead of Instantiate to support specifying null values
        public static T InstantiatePrefabForComponentExplicit<T>(
            this IInstantiator container, GameObject prefab, List<TypeValuePair> extraArgMap)
        {
            return (T)container.InstantiatePrefabForComponentExplicit(typeof(T), prefab, extraArgMap);
        }

        public static object InstantiatePrefabForComponentExplicit(
            this IInstantiator container, Type concreteType, GameObject prefab, List<TypeValuePair> extraArgMap)
        {
            Assert.IsType<DiContainer>(container);
            return container.InstantiatePrefabForComponentExplicit(
                concreteType, prefab, extraArgMap, new InjectContext((DiContainer)container, concreteType, null));
        }

        /////////////// InstantiateComponentOnNewGameObject

        public static T InstantiateComponentOnNewGameObject<T>(
            this IInstantiator container, string name, params object[] extraArgs)
        {
            return (T)container.InstantiateComponentOnNewGameObject(typeof(T), name, extraArgs);
        }

        public static object InstantiateComponentOnNewGameObject(
            this IInstantiator container, Type concreteType, string name, params object[] extraArgs)
        {
            Assert.That(!extraArgs.Contains(null),
                "Null value given to factory constructor arguments when instantiating object with type '{0}'. In order to use null use InstantiateComponentOnNewGameObjectExplicit", concreteType);

            return container.InstantiateComponentOnNewGameObjectExplicit(
                concreteType, name, InstantiateUtil.CreateTypeValueList(extraArgs));
        }

        // This is used instead of Instantiate to support specifying null values
        public static T InstantiateComponentOnNewGameObjectExplicit<T>(
            this IInstantiator container, string name, List<TypeValuePair> extraArgMap)
        {
            return (T)container.InstantiateComponentOnNewGameObjectExplicit(typeof(T), name, extraArgMap);
        }

        public static object InstantiateComponentOnNewGameObjectExplicit(
            this IInstantiator container, Type concreteType, string name, List<TypeValuePair> extraArgMap)
        {
            Assert.IsType<DiContainer>(container);
            return container.InstantiateComponentOnNewGameObjectExplicit(
                concreteType, name, extraArgMap, new InjectContext((DiContainer)container, concreteType, null));
        }
#endif

        public static T Instantiate<T>(
            this IInstantiator container, params object[] extraArgs)
        {
            return (T)container.Instantiate(typeof(T), extraArgs);
        }

        public static object Instantiate(
            this IInstantiator container, Type concreteType, params object[] extraArgs)
        {
            Assert.That(!extraArgs.Contains(null),
                "Null value given to factory constructor arguments when instantiating object with type '{0}'. In order to use null use InstantiateExplicit", concreteType);

            return container.InstantiateExplicit(
                concreteType, InstantiateUtil.CreateTypeValueList(extraArgs));
        }

        // This is used instead of Instantiate to support specifying null values
        public static T InstantiateExplicit<T>(
            this IInstantiator container, List<TypeValuePair> extraArgMap)
        {
            return (T)container.InstantiateExplicit(typeof(T), extraArgMap);
        }

        public static object InstantiateExplicit(
            this IInstantiator container, Type concreteType, List<TypeValuePair> extraArgMap)
        {
            Assert.IsType<DiContainer>(container);
            return container.InstantiateExplicit(
                concreteType, extraArgMap, new InjectContext((DiContainer)container, concreteType, null), null, true);
        }
    }
}
