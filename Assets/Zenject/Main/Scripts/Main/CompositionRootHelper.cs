#if !ZEN_NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public static class CompositionRootHelper
    {
        public static void InstallStandardInstaller(DiContainer container, GameObject rootObj)
        {
            container.Bind<GameObject>().ToInstance(rootObj).WhenInjectedInto<StandardUnityInstaller>();
            container.Install<StandardUnityInstaller>();
        }

        public static void InstallSceneInstallers(
            DiContainer container, IEnumerable<IInstaller> installers)
        {
            foreach (var installer in installers)
            {
                if (installer == null)
                {
                    Log.Warn("Found null installer in composition root");
                    continue;
                }

                if (installer.IsEnabled)
                {
                    container.Install(installer);
                }
            }
        }
    }
}

#endif
