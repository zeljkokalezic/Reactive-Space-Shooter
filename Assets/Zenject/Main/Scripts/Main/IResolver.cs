using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;

#if !ZEN_NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public interface IResolver
    {
        IList ResolveAll(InjectContext context);

        List<Type> ResolveTypeAll(InjectContext context);

        object Resolve(InjectContext context);

        void InjectExplicit(
            object injectable, IEnumerable<TypeValuePair> extraArgs,
            bool shouldUseAll, ZenjectTypeInfo typeInfo, InjectContext context, string concreteIdentifier);
    }

    public static class ResolverExtensions
    {
#if !ZEN_NOT_UNITY3D
        // Inject dependencies into child game objects
        public static void InjectGameObject(
            this IResolver container, GameObject gameObject, bool recursive, bool includeInactive)
        {
            container.InjectGameObject(gameObject, recursive, includeInactive, Enumerable.Empty<object>());
        }

        public static void InjectGameObject(
            this IResolver container, GameObject gameObject, bool recursive)
        {
            container.InjectGameObject(gameObject, recursive, false);
        }

        public static void InjectGameObject(
            this IResolver container, GameObject gameObject)
        {
            container.InjectGameObject(gameObject, true, false);
        }

        public static void InjectGameObject(
            this IResolver container, GameObject gameObject,
            bool recursive, bool includeInactive, IEnumerable<object> extraArgs)
        {
            container.InjectGameObject(
                gameObject, recursive, includeInactive, extraArgs, null);
        }

        public static void InjectGameObject(
            this IResolver container, GameObject gameObject,
            bool recursive, bool includeInactive, IEnumerable<object> extraArgs, InjectContext context)
        {
            IEnumerable<MonoBehaviour> components;

            if (recursive)
            {
                components = UnityUtil.GetComponentsInChildrenBottomUp<MonoBehaviour>(gameObject, includeInactive);
            }
            else
            {
                if (!includeInactive && !gameObject.activeSelf)
                {
                    return;
                }

                components = gameObject.GetComponents<MonoBehaviour>();
            }

            foreach (var component in components)
            {
                // null if monobehaviour link is broken
                if (component != null)
                {
                    container.Inject(component, extraArgs, false, context);
                }
            }
        }
#endif

        public static void Inject(this IResolver container, object injectable)
        {
            container.Inject(injectable, Enumerable.Empty<object>());
        }

        public static void Inject(this IResolver container, object injectable, IEnumerable<object> additional)
        {
            container.Inject(injectable, additional, true);
        }

        public static void Inject(this IResolver container, object injectable, IEnumerable<object> additional, bool shouldUseAll)
        {
            container.Inject(
                injectable, additional, shouldUseAll, new InjectContext((DiContainer)container, injectable.GetType(), null));
        }

        internal static void Inject(
            this IResolver container, object injectable, IEnumerable<object> additional, bool shouldUseAll, InjectContext context)
        {
            container.Inject(
                injectable, additional, shouldUseAll, context, TypeAnalyzer.GetInfo(injectable.GetType()));
        }

        internal static void Inject(
            this IResolver container, object injectable,
            IEnumerable<object> additional, bool shouldUseAll, InjectContext context, ZenjectTypeInfo typeInfo)
        {
            Assert.That(!additional.Contains(null),
                "Null value given to injection argument list. In order to use null you must provide a List<TypeValuePair> and not just a list of objects");

            container.InjectExplicit(
                injectable, InstantiateUtil.CreateTypeValueList(additional), shouldUseAll, typeInfo, context, null);
        }

        public static void InjectExplicit(this IResolver container, object injectable, List<TypeValuePair> additional)
        {
            container.InjectExplicit(
                injectable, additional, new InjectContext((DiContainer)container, injectable.GetType(), null));
        }

        public static void InjectExplicit(this IResolver container, object injectable, List<TypeValuePair> additional, InjectContext context)
        {
            container.InjectExplicit(
                injectable, additional, true,
                TypeAnalyzer.GetInfo(injectable.GetType()), context, null);
        }

        public static List<Type> ResolveTypeAll(this IResolver container, Type type)
        {
            return container.ResolveTypeAll(new InjectContext((DiContainer)container, type, null));
        }

        public static TContract Resolve<TContract>(this IResolver container)
        {
            return container.Resolve<TContract>((string)null);
        }

        public static TContract Resolve<TContract>(this IResolver container, string identifier)
        {
            return container.Resolve<TContract>(new InjectContext((DiContainer)container, typeof(TContract), identifier));
        }

        public static TContract TryResolve<TContract>(this IResolver container)
            where TContract : class
        {
            return container.TryResolve<TContract>((string)null);
        }

        public static TContract TryResolve<TContract>(this IResolver container, string identifier)
            where TContract : class
        {
            return (TContract)container.TryResolve(typeof(TContract), identifier);
        }

        public static object TryResolve(this IResolver container, Type contractType)
        {
            return container.TryResolve(contractType, null);
        }

        public static object TryResolve(this IResolver container, Type contractType, string identifier)
        {
            return container.Resolve(new InjectContext((DiContainer)container, contractType, identifier, true));
        }

        public static object Resolve(this IResolver container, Type contractType)
        {
            return container.Resolve(new InjectContext((DiContainer)container, contractType, null));
        }

        public static object Resolve(this IResolver container, Type contractType, string identifier)
        {
            return container.Resolve(new InjectContext((DiContainer)container, contractType, identifier));
        }

        public static TContract Resolve<TContract>(this IResolver container, InjectContext context)
        {
            Assert.IsEqual(context.MemberType, typeof(TContract));
            return (TContract) container.Resolve(context);
        }

        public static List<TContract> ResolveAll<TContract>(this IResolver container)
        {
            return container.ResolveAll<TContract>((string)null);
        }

        public static List<TContract> ResolveAll<TContract>(this IResolver container, bool optional)
        {
            return container.ResolveAll<TContract>(null, optional);
        }

        public static List<TContract> ResolveAll<TContract>(this IResolver container, string identifier)
        {
            return container.ResolveAll<TContract>(identifier, false);
        }

        public static List<TContract> ResolveAll<TContract>(this IResolver container, string identifier, bool optional)
        {
            var context = new InjectContext((DiContainer)container, typeof(TContract), identifier, optional);
            return container.ResolveAll<TContract>(context);
        }

        public static List<TContract> ResolveAll<TContract>(this IResolver container, InjectContext context)
        {
            Assert.IsEqual(context.MemberType, typeof(TContract));
            return (List<TContract>) container.ResolveAll(context);
        }

        public static IList ResolveAll(this IResolver container, Type contractType)
        {
            return container.ResolveAll(contractType, null);
        }

        public static IList ResolveAll(this IResolver container, Type contractType, string identifier)
        {
            return container.ResolveAll(contractType, identifier, false);
        }

        public static IList ResolveAll(this IResolver container, Type contractType, bool optional)
        {
            return container.ResolveAll(contractType, null, optional);
        }

        public static IList ResolveAll(this IResolver container, Type contractType, string identifier, bool optional)
        {
            var context = new InjectContext((DiContainer)container, contractType, identifier, optional);
            return container.ResolveAll(context);
        }
    }
}
