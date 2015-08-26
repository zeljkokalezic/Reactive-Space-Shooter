using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class ReactiveInstaller : MonoInstaller{

    public Settings sceneSettings;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().ToSingle();
        Container.Bind<GameModel>().ToSingle();

        Container.Bind<EnemyModel>().ToTransient();
        Container.Bind<WeaponModel>().ToTransient();

        Container.Bind<PlayerModel.Settings>().ToSingleInstance(sceneSettings.playerSettings);
        Container.Bind<EnemyPresenter.Settings>().ToSingleInstance(sceneSettings.enemyPresenterSettings).WhenInjectedInto<EnemyPresenter>();
        Container.Bind<WeaponModel.Settings>().ToSingleInstance(sceneSettings.playerWeaponSettings).WhenInjectedInto<WeaponModel>();
        Container.Bind<EnemyModel.Settings>().ToSingleInstance(sceneSettings.enemySettings).WhenInjectedInto<EnemyModel>();

        Container.Bind<WeaponModel.Factory>().ToSingle();
        Container.Bind<EnemyPresenter.Factory>().ToSingle();
        Container.Bind<WeaponPresenter.Factory>().ToSingle();

        //inteligent installer ! - rework
        //this is not good !
        Container.Bind<WeaponSpawner>().ToSinglePrefab<WeaponSpawner>(sceneSettings.weaponSpawner).WhenInjectedInto<PlayerPresenter>();
    }

    [Serializable]
    public class Settings
    {        
        public GameObject weaponSpawner;
        public PlayerModel.Settings playerSettings;
        public EnemyModel.Settings enemySettings;
        public WeaponModel.Settings playerWeaponSettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
    }
}
