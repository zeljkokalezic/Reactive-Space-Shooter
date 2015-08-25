using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class ReactiveInstaller : MonoInstaller{

    public Settings sceneSettings;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().ToSingle();
        Container.Bind<PlayerModel.Settings>().ToSingleInstance(sceneSettings.playerSettings);
        Container.Bind<EnemyPresenter.Settings>().ToSingleInstance(sceneSettings.enemyPresenterSettings).WhenInjectedInto<EnemyPresenter>();
        Container.Bind<GameModel>().ToSingle();
        Container.Bind<EnemyModel>().ToTransient();
        Container.Bind<WeaponModel>().ToTransient();
        Container.Bind<WeaponModel.Settings>().ToSingleInstance(sceneSettings.playerWeaponSettings).WhenInjectedInto<WeaponModel>();
        
        //bind to empty(default) prefab - spawner will take care of instatiating correct prefab later
        Container.BindGameObjectFactory<EnemyPresenter.Factory>(sceneSettings.defaultPrefab);
        Container.BindGameObjectFactory<WeaponPresenter.Factory>(sceneSettings.defaultPrefab);

        Container.Bind<WeaponSpawner>().ToSinglePrefab<WeaponSpawner>(sceneSettings.weaponSpawner).WhenInjectedInto<PlayerPresenter>();
    }

    [Serializable]
    public class Settings
    {
        public GameObject defaultPrefab;
        public GameObject weaponSpawner;
        public PlayerModel.Settings playerSettings;
        public WeaponModel.Settings playerWeaponSettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
    }
}
