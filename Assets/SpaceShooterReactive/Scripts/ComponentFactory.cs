using System;
using System.Collections.Generic;
using Zenject;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class SimpleComponentFactory
    {
        [Inject]
        protected readonly DiContainer _container;

        public T Create<T>(GameObject gameObject) where T : Component
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject);
        }

        public T Create<T>(GameObject gameObject, params object[] args) where T : Component
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject, args);
        }
    }

    //No parameters
    public class ComponentFactory<T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public T Create(GameObject gameObject)
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject);
        }

        public System.Collections.Generic.IEnumerable<ZenjectResolveException> Validate()
        {
            return _container.ValidateObjectGraph<T>();
        }
    }

    // One parameter
    public class ComponentFactory<P1, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject gameObject, P1 param)
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject, param);
        }

        public System.Collections.Generic.IEnumerable<ZenjectResolveException> Validate()
        {
            return _container.ValidateObjectGraph<T>(typeof(P1));
        }
    }

    // Two parameters
    public class ComponentFactory<P1, P2, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject gameObject, P1 param, P2 param2)
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject, param, param2);
        }

        public System.Collections.Generic.IEnumerable<ZenjectResolveException> Validate()
        {
            return _container.ValidateObjectGraph<T>(typeof(P1), typeof(P2));
        }
    }

    // Three parameters
    public class ComponentFactory<P1, P2, P3, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject gameObject, P1 param, P2 param2, P3 param3)
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject, param, param2, param3);
        }

        public System.Collections.Generic.IEnumerable<ZenjectResolveException> Validate()
        {
            return _container.ValidateObjectGraph<T>(typeof(P1), typeof(P2), typeof(P3));
        }
    }

    // Four parameters
    public class ComponentFactory<P1, P2, P3, P4, T> : IValidatable
        where T : Component
    {
        [Inject]
        protected readonly DiContainer _container;

        public virtual T Create(GameObject gameObject, P1 param, P2 param2, P3 param3, P4 param4)
        {
            Assert.That(gameObject != null,
               "Null component given to factory create method when instantiating component with type '{0}'.", typeof(T));

            return _container.InstantiateComponent<T>(gameObject, param, param2, param3, param4);
        }

        public System.Collections.Generic.IEnumerable<ZenjectResolveException> Validate()
        {
            return _container.ValidateObjectGraph<T>(typeof(P1), typeof(P2), typeof(P3), typeof(P4));
        }
    }
}