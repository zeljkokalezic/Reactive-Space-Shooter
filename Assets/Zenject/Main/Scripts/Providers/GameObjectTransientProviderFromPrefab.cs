#if !ZEN_NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class GameObjectTransientProviderFromPrefab<T> : ProviderBase where T : Component
    {
        DiContainer _container;
        GameObject _template;

        public GameObjectTransientProviderFromPrefab(DiContainer container, GameObject template)
        {
            _container = container;
            _template = template;
        }

        public override Type GetInstanceType()
        {
            return typeof(T);
        }

        public override object GetInstance(InjectContext context)
        {
            Assert.That(typeof(T).DerivesFromOrEqual(context.MemberType));
            return _container.InstantiatePrefabForComponent<T>(_template);
        }

        public override IEnumerable<ZenjectResolveException> ValidateBinding(InjectContext context)
        {
            return BindingValidator.ValidateObjectGraph(_container, typeof(T), context, null);
        }
    }
}

#endif
