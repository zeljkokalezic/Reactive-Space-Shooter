#if !ZEN_NOT_UNITY3D

#pragma warning disable 414
using ModestTree;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zenject
{
    public sealed class GlobalCompositionRoot : MonoBehaviour
    {
        static GlobalCompositionRoot _instance;
        DiContainer _container;
        IDependencyRoot _dependencyRoot;

        public DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static GlobalCompositionRoot Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("Global Composition Root")
                        .AddComponent<GlobalCompositionRoot>();
                }
                return _instance;
            }
        }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            // Is this a good idea?
            //go.hideFlags = HideFlags.HideInHierarchy;

            _container = CreateContainer(false, gameObject);
            _dependencyRoot = _container.Resolve<IDependencyRoot>();
        }

        // If we're destroyed manually somehow handle that
        public void OnDestroy()
        {
            _instance = null;
            _dependencyRoot = null;
        }

        public static DiContainer CreateContainer(bool allowNullBindings, GameObject gameObj)
        {
            Assert.That(allowNullBindings || gameObj != null);

            var container = new DiContainer(gameObj == null ? null : gameObj.transform);

            container.AllowNullBindings = allowNullBindings;

            CompositionRootHelper.InstallStandardInstaller(container, gameObj);
            CompositionRootHelper.InstallSceneInstallers(container, GetGlobalInstallers());

            return container;
        }

        static IEnumerable<IInstaller> GetGlobalInstallers()
        {
            var installerConfig = (GlobalInstallerConfig)Resources.Load("ZenjectGlobalCompositionRoot", typeof(GlobalInstallerConfig));

            if (installerConfig == null)
            {
                return Enumerable.Empty<IInstaller>();
            }

            return installerConfig.Installers;
        }
    }
}

#endif
