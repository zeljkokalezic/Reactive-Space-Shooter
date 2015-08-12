#if !ZEN_NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;
using UnityEngine;

namespace Zenject
{
    public class PrefabSingletonLazyCreator
    {
        readonly DiContainer _container;
        readonly PrefabSingletonProviderMap _owner;
        readonly PrefabSingletonId _id;

        int _referenceCount;
        GameObject _rootObj;

        public PrefabSingletonLazyCreator(
            DiContainer container, PrefabSingletonProviderMap owner,
            PrefabSingletonId id)
        {
            _container = container;
            _owner = owner;
            _id = id;

            Assert.IsNotNull(id.Prefab);
        }

        public GameObject Prefab
        {
            get
            {
                return _id.Prefab;
            }
        }

        public GameObject RootObject
        {
            get
            {
                return _rootObj;
            }
        }

        public void IncRefCount()
        {
            _referenceCount += 1;
        }

        public void DecRefCount()
        {
            _referenceCount -= 1;

            if (_referenceCount <= 0)
            {
                _owner.RemoveCreator(_id);
            }
        }

        public IEnumerable<Type> GetAllComponentTypes()
        {
            return _id.Prefab.GetComponentsInChildren<Component>(true).Where(x => x != null).Select(x => x.GetType());
        }

        public bool ContainsComponent(Type type)
        {
            return !_id.Prefab.GetComponentsInChildren(type, true).IsEmpty();
        }

        public object GetComponent(Type componentType, InjectContext context)
        {
            if (_rootObj == null)
            {
                _rootObj = (GameObject)GameObject.Instantiate(_id.Prefab);

                // Default parent to comp root
                _rootObj.transform.SetParent(_container.Resolve<CompositionRoot>().transform, false);
                _rootObj.SetActive(true);

                _container.InjectGameObject(_rootObj, true, false, new object[0], context);
            }

            var component = _rootObj.GetComponentInChildren(componentType);

            if (component == null)
            {
                throw new ZenjectResolveException(
                    "Could not find component with type '{0}' in given singleton prefab".Fmt(componentType));
            }

            return component;
        }
    }
}

#endif
