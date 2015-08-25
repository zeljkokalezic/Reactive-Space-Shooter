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
        public float fireRate;
        public Transform weaponMountPoint;
    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerSpeed { get; private set; }
    public ReactiveProperty<float> RxPlayerFireRate { get; private set; }

    //this should be in the weapon model
    //public ReactiveProperty<Transform> RxPlayerWeaponMountPoint { get; private set; }

    public WeaponModel PlayerWeapon { get; private set; }

    [Inject]
    public PlayerModel(Settings playerSettings, WeaponModel playerWeapon)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerSpeed = new ReactiveProperty<int>(playerSettings.speed);
        RxPlayerFireRate = new ReactiveProperty<float>(playerSettings.fireRate);
        //RxPlayerWeaponMountPoint = new ReactiveProperty<Transform>(playerSettings.weaponMountPoint);
        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);

        PlayerWeapon = playerWeapon;
        PlayerWeapon.RxPlayerWeaponMountPoint.Value = playerSettings.weaponMountPoint;
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
    }
}
