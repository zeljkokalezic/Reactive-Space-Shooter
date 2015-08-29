using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class MainInstaller : MonoInstaller{

    public Settings sceneSettings;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().ToSingle();
        Container.Bind<GameModel>().ToSingle();

        Container.Bind<EnemyModel>().ToTransient();
        Container.Bind<WeaponModel>().ToTransient();

        Container.Bind<PlayerModel.Settings>().ToSingleInstance(sceneSettings.playerSettings).WhenInjectedInto<PlayerModel>();
        Container.Bind<PlayerPresenter.Settings>().ToSingleInstance(sceneSettings.playerPresenterSettings).WhenInjectedInto<PlayerPresenter>();
        Container.Bind<EnemyPresenter.Settings>().ToSingleInstance(sceneSettings.enemyPresenterSettings).WhenInjectedInto<EnemyPresenter>();
        //Container.Bind<WeaponModel.Settings>().ToSingleInstance(sceneSettings.playerWeaponSettings).WhenInjectedInto<WeaponModel>();
        Container.Bind<EnemyModel.Settings>().ToSingleInstance(sceneSettings.enemySettings).WhenInjectedInto<EnemyModel>();
        Container.Bind<ShipDriverPlayer.Settings>().ToSingleInstance(sceneSettings.shipDriverPlayerSettings).WhenInjectedInto<ShipDriverPlayer>();
        Container.Bind<ShipDriverAI.Settings>().ToSingleInstance(sceneSettings.shipDriverAISettings).WhenInjectedInto<ShipDriverAI>();

        Container.Bind<WeaponModel.Factory>().ToSingle();
        Container.Bind<EnemyModel.Factory>().ToSingle();
        Container.Bind<EnemyPresenter.Factory>().ToSingle();
        Container.Bind<WeaponBulletPresenter.Factory>().ToSingle();
        Container.Bind<WeaponPresenter.Factory>().ToSingle();
        Container.Bind<SimpleComponentFactory>().ToSingle();        
    }

    [Serializable]
    public class Settings
    {        
        public PlayerModel.Settings playerSettings;
        public EnemyModel.Settings enemySettings;
        //public WeaponModel.Settings playerWeaponSettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
        public PlayerPresenter.Settings playerPresenterSettings;
        public ShipDriverPlayer.Settings shipDriverPlayerSettings;
        public ShipDriverAI.Settings shipDriverAISettings;
    }
}
