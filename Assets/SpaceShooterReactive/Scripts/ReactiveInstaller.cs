using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class ReactiveInstaller : MonoInstaller{

    public Settings SceneSettings;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().ToSingle();
        Container.Bind<PlayerModel.Settings>().ToSingleInstance(SceneSettings.playerSettings);
        Container.Bind<EnemyPresenter.Settings>().ToSingleInstance(SceneSettings.enemyPresenterSettings).WhenInjectedInto<EnemyPresenter>();
        Container.Bind<GameModel>().ToSingle();
        Container.Bind<EnemyModel>().ToTransient();
        Container.BindGameObjectFactory<EnemyPresenter.Factory>(SceneSettings.asteroidPrefab);
    }

    [Serializable]
    public class Settings
    {
        public GameObject asteroidPrefab;
        public PlayerModel.Settings playerSettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
    }
}
