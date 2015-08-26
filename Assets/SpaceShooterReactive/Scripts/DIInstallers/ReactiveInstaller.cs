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
        Container.Bind<WeaponModel>().ToTransient();//ToSingle();//we have just one weapon for now, find a solution for transient weapons
        Container.Bind<WeaponModel.Factory>().ToSingle();
        Container.Bind<WeaponModel.Settings>().ToSingleInstance(sceneSettings.playerWeaponSettings).WhenInjectedInto<WeaponModel>();
        Container.Bind<EnemyModel.Settings>().ToSingleInstance(sceneSettings.enemySettings).WhenInjectedInto<EnemyModel>();

        Container.Bind<EnemyPresenter.Factory>().ToSingle();
        Container.Bind<WeaponPresenter.Factory>().ToSingle();
        //bind to empty(default) prefab - spawner will take care of instatiating correct prefab later
        //Container.BindGameObjectFactory<EnemyPresenter.Factory>(sceneSettings.defaultPrefab);
        //Container.BindGameObjectFactory<WeaponPresenter.Factory>(sceneSettings.defaultPrefab);

        //inteligent installer ! - rework
        Container.Bind<WeaponSpawner>().ToSinglePrefab<WeaponSpawner>(sceneSettings.weaponSpawner).WhenInjectedInto<PlayerPresenter>();
    }

    [Serializable]
    public class Settings
    {
        //public GameObject defaultPrefab;
        public GameObject weaponSpawner;
        public PlayerModel.Settings playerSettings;
        public EnemyModel.Settings enemySettings;
        public WeaponModel.Settings playerWeaponSettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
    }
}
