#if !ZEN_NOT_UNITY3D

using System;
using ModestTree;
using ModestTree.Util.Debugging;
using UnityEngine;

namespace Zenject
{
    public sealed class SceneDecoratorCompositionRoot : MonoBehaviour
    {
        public string SceneName;

        [SerializeField]
        public DecoratorInstaller[] DecoratorInstallers;

        [SerializeField]
        public MonoInstaller[] PreInstallers;

        [SerializeField]
        public MonoInstaller[] PostInstallers;

        Action<DiContainer> _beforeInstallHooks;
        Action<DiContainer> _afterInstallHooks;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _beforeInstallHooks = CompositionRoot.BeforeInstallHooks;
            CompositionRoot.BeforeInstallHooks = null;

            _afterInstallHooks = CompositionRoot.AfterInstallHooks;
            CompositionRoot.AfterInstallHooks = null;

            ZenUtil.LoadScene(
                SceneName, AddPreBindings, AddPostBindings);
        }

        public void AddPreBindings(DiContainer container)
        {
            if (_beforeInstallHooks != null)
            {
                _beforeInstallHooks(container);
                _beforeInstallHooks = null;
            }

            // Make our scene graph a child of the new CompositionRoot so any monobehaviour's that are
            // built into the scene get injected
            transform.parent = container.Resolve<CompositionRoot>().transform;

            CompositionRootHelper.InstallSceneInstallers(container, PreInstallers);

            ProcessDecoratorInstallers(container, true);
        }

        public void AddPostBindings(DiContainer container)
        {
            CompositionRootHelper.InstallSceneInstallers(container, PostInstallers);

            ProcessDecoratorInstallers(container, false);

            if (_afterInstallHooks != null)
            {
                _afterInstallHooks(container);
                _afterInstallHooks = null;
            }
        }

        void ProcessDecoratorInstallers(DiContainer container, bool isBefore)
        {
            if (DecoratorInstallers == null)
            {
                return;
            }

            foreach (var installer in DecoratorInstallers)
            {
                if (installer == null)
                {
                    Log.Warn("Found null installer in composition root");
                    continue;
                }

                if (installer.enabled)
                {
                    container.Inject(installer);

                    if (isBefore)
                    {
                        installer.PreInstallBindings();
                    }
                    else
                    {
                        installer.PostInstallBindings();
                    }
                }
            }
        }
    }
}

#endif
