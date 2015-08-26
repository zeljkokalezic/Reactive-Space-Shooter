using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class PlayerModel
{
    [Serializable]
    public class Settings
    {
        public string playerName;
        public int speed;
        public int score;
        public float fireRate;
        public WeaponModel.Settings weaponSettings;
        //public Transform weaponMountPoint;
    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerSpeed { get; private set; }
    public ReactiveProperty<int> RxPlayerScore { get; private set; }
    public ReactiveProperty<float> RxPlayerFireRate { get; private set; }

    public WeaponModel PlayerWeapon { get; private set; }

    [Inject]
    public PlayerModel(Settings playerSettings, WeaponModel.Factory playerWeaponFactory)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerSpeed = new ReactiveProperty<int>(playerSettings.speed);
        RxPlayerScore = new ReactiveProperty<int>(playerSettings.score);
        RxPlayerFireRate = new ReactiveProperty<float>(playerSettings.fireRate);
        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);

        //do we need a weapon factory(spawner) here ? <- Probably yes, DI will autolink
        PlayerWeapon = playerWeaponFactory.Create(this);
        //we should create auto linking somehow ...
        //player is already injected into weapon becouse it is singleton for now, factory later
        //playerWeapon.Player = this;
        //PlayerWeapon.RxPlayerWeaponMountPoint.Value = playerSettings.weaponMountPoint;        
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
    }

    internal void WeaponHit(WeaponModel weaponModel, EnemyModel enemyModel)
    {
        RxPlayerScore.Value += enemyModel.RxEnemyScore.Value;
    }
}
