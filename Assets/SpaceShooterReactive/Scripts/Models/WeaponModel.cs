using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class WeaponModel
{
    public class Factory : Factory<PlayerModel, Settings, WeaponModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public GameObject bullet;
    }

    public ReactiveProperty<Transform> RxPlayerWeaponMountPoint { get; private set; }
    public ReactiveProperty<GameObject> RxPlayerWeaponBullet { get; private set; }

    [Inject]
    public PlayerModel Player { get; set; }

    [Inject]
    public WeaponModel(Settings weaponSettings)
    {
        RxPlayerWeaponMountPoint = new ReactiveProperty<Transform>();
        RxPlayerWeaponBullet = new ReactiveProperty<GameObject>(weaponSettings.bullet);
    }

    public void Hit(EnemyModel enemyModel)
    {
        Player.WeaponHit(this, enemyModel);
    }
}
