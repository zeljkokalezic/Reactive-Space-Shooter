using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class WeaponModel
{
    public class Factory : Factory<PlayerModel, WeaponModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public Transform weaponMountPoint;
    }

    public ReactiveProperty<Transform> RxPlayerWeaponMountPoint { get; private set; }

    [Inject]
    public PlayerModel Player { get; set; }

    [Inject]
    public WeaponModel(Settings playerSettings)
    {
        RxPlayerWeaponMountPoint = new ReactiveProperty<Transform>(playerSettings.weaponMountPoint);
    }

    public void Hit(EnemyModel enemyModel)
    {
        Player.WeaponHit(this, enemyModel);
    }
}
