using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class PlayerModel: IArmed, IDamageable
{
    [Serializable]
    public class Settings
    {
        public string playerName;
        public int speed;
        public int initialScore;
        public WeaponModel.Settings weaponSettings;
    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerSpeed { get; private set; }
    public ReactiveProperty<int> RxPlayerScore { get; private set; }

    //IDamageable
    public ReactiveProperty<int> RxHealth { get; set; }
    public ReactiveProperty<int> RxScore { get; set; }

    public WeaponModel PlayerWeapon { get; private set; }
    public ShipModel PlayerShip { get; private set; }

    [Inject]
    public PlayerModel(Settings playerSettings, WeaponModel.Factory weaponFactory)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerSpeed = new ReactiveProperty<int>(playerSettings.speed);
        RxPlayerScore = new ReactiveProperty<int>(playerSettings.initialScore);
        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);

        //now we have ability to instantiate multiple weapons
        PlayerWeapon = weaponFactory.Create(this, playerSettings.weaponSettings);
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
        PlayerWeapon.RxWeaponState.Value = WeaponModel.WeaponState.Active;//activate weapon
    }

    internal void Deactivate()
    {
        RxPlayerState.Value = PlayerState.Dead;//do we need the inactive state ?
        PlayerWeapon.RxWeaponState.Value = WeaponModel.WeaponState.Inactive;
    }

    public void HitByWeapon(WeaponModel weaponModel, IArmed other)
    {
        Deactivate();
    }

    public void WeaponHit(WeaponModel weaponModel, IDamageable other)
    {
        //should the interface hold the health or just state (dead/alive) ?
        if (other.RxHealth.Value == 0)//if the enemy is dead increase the score
        {
            RxPlayerScore.Value += other.RxScore.Value;
        }
    }
}
