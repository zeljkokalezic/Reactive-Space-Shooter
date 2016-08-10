using UnityEngine;
using System.Collections;
using Zenject;
using System;

public class MainInstaller : MonoInstaller {

    //Fill this from config file (save game) later
    public Settings sceneSettings;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().ToSingle();
        Container.Bind<GameModel>().ToSingle();

        Container.Bind<ShipModel>().ToTransient();
        Container.Bind<EnemyModel>().ToTransient();
        Container.Bind<WeaponModel>().ToTransient();

        Container.Bind<PlayerModel.Settings>().ToSingleInstance(sceneSettings.playerSettings).WhenInjectedInto<PlayerModel>();
        Container.Bind<PlayerPresenter.Settings>().ToSingleInstance(sceneSettings.playerPresenterSettings).WhenInjectedInto<PlayerPresenter>();
        Container.Bind<EnemyPresenter.Settings>().ToSingleInstance(sceneSettings.enemyPresenterSettings).WhenInjectedInto<EnemyPresenter>();
        Container.Bind<EnemyModel.Settings>().ToSingleInstance(sceneSettings.enemySettings);        
        Container.Bind<ShipDriverPlayer.Settings>().ToSingleInstance(sceneSettings.shipDriverPlayerSettings).WhenInjectedInto<ShipDriverPlayer>();
        Container.Bind<ShipDriverAI.Settings>().ToSingleInstance(sceneSettings.shipDriverAISettings).WhenInjectedInto<ShipDriverAI>();

        Container.Bind<ShipPresenter.Settings>().ToTransient();
        Container.Bind<ShipModel.Settings>().ToTransient();

        Container.Bind<WeaponModel.Factory>().ToSingle();
        Container.Bind<EnemyModel.Factory>().ToSingle();
        Container.Bind<EnemyPresenter.Factory>().ToSingle();
        Container.Bind<WeaponBulletPresenter.Factory>().ToSingle();
        Container.Bind<WeaponPresenter.Factory>().ToSingle();
        Container.Bind<ShipModel.Factory>().ToSingle();
        Container.Bind<ShipPresenter.Factory>().ToSingle();
        Container.Bind<HealthBar.Factory>().ToSingle();
        Container.Bind<SimpleComponentFactory>().ToSingle();
    }

    [Serializable]
    public class Settings
    {        
        public PlayerModel.Settings playerSettings;
        public PlayerPresenter.Settings playerPresenterSettings;
        public EnemyModel.Settings enemySettings;
        public EnemyPresenter.Settings enemyPresenterSettings;
        public ShipDriverPlayer.Settings shipDriverPlayerSettings;
        public ShipDriverAI.Settings shipDriverAISettings;
    }
}
