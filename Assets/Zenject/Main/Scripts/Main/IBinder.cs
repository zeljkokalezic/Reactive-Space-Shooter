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
    public interface IBinder
    {
        bool Unbind<TContract>(string identifier);

        bool HasBinding(InjectContext context);

        BinderUntyped Bind(Type contractType, string identifier);
        BinderGeneric<TContract> Bind<TContract>(string identifier);

        BinderGeneric<TContract> Rebind<TContract>();

        IFactoryBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>(string identifier);
        IFactoryBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>();

        IFactoryBinder<TParam1, TParam2, TParam3, TContract> BindIFactory<TParam1, TParam2, TParam3, TContract>(string identifier);
        IFactoryBinder<TParam1, TParam2, TParam3, TContract> BindIFactory<TParam1, TParam2, TParam3, TContract>();

        IFactoryBinder<TParam1, TParam2, TContract> BindIFactory<TParam1, TParam2, TContract>(string identifier);
        IFactoryBinder<TParam1, TParam2, TContract> BindIFactory<TParam1, TParam2, TContract>();

        IFactoryBinder<TParam1, TContract> BindIFactory<TParam1, TContract>(string identifier);
        IFactoryBinder<TParam1, TContract> BindIFactory<TParam1, TContract>();

        IFactoryBinder<TContract> BindIFactory<TContract>(string identifier);
        IFactoryBinder<TContract> BindIFactory<TContract>();

        IFactoryUntypedBinder<TContract> BindIFactoryUntyped<TContract>(string identifier);
        IFactoryUntypedBinder<TContract> BindIFactoryUntyped<TContract>();
    }

    public static class BinderExtensions
    {
        public static BindingConditionSetter BindInstance<TContract>(this DiContainer container, string identifier, TContract obj)
        {
            return container.Bind<TContract>(identifier).ToInstance(obj);
        }

        public static BindingConditionSetter BindInstance<TContract>(this DiContainer container, TContract obj)
        {
            return container.Bind<TContract>().ToInstance(obj);
        }

        public static BinderGeneric<TContract> Bind<TContract>(this DiContainer container)
        {
            return container.Bind<TContract>(null);
        }

        public static BinderUntyped Bind(this DiContainer container, Type contractType)
        {
            return container.Bind(contractType, null);
        }

        public static bool Unbind<TContract>(this DiContainer container)
        {
            return container.Unbind<TContract>(null);
        }

        public static bool HasBinding<TContract>(this DiContainer container)
        {
            return container.HasBinding<TContract>(null);
        }

        public static bool HasBinding<TContract>(this DiContainer container, string identifier)
        {
            return container.HasBinding(
                new InjectContext(container, typeof(TContract), identifier));
        }

        public static void BindAllInterfacesToSingle<TConcrete>(this DiContainer container)
        {
            container.BindAllInterfacesToSingle(typeof(TConcrete));
        }

        public static void BindAllInterfacesToSingle(this DiContainer container, Type concreteType)
        {
            foreach (var interfaceType in concreteType.GetInterfaces())
            {
                Assert.That(concreteType.DerivesFrom(interfaceType));
                container.Bind(interfaceType).ToSingle(concreteType);
            }
        }

#if !ZEN_NOT_UNITY3D
        public static BindingConditionSetter BindGameObjectFactory<T>(
            this DiContainer container, GameObject prefab)
            // This would be useful but fails with VerificationException's in webplayer builds for some reason
            //where T : GameObjectFactory
            where T : class
        {
            if (prefab == null)
            {
                throw new ZenjectBindException(
                    "Null prefab provided to BindGameObjectFactory for type '{0}'".Fmt(typeof(T).Name()));
            }

            // We could bind the factory ToSingle but doing it this way is better
            // since it allows us to have multiple game object factories that
            // use different prefabs and have them injected into different places
            return container.Bind<T>().ToMethod((ctx) => ctx.Container.Instantiate<T>(prefab));
        }
#endif
    }
}

