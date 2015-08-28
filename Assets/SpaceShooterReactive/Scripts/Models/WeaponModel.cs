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
        
    }

    public ReactiveProperty<Transform> RxPlayerWeaponMountPoint { get; private set; }

    [Inject]
    public PlayerModel Player { get; set; }

    [Inject]
    public WeaponModel(Settings weaponSettings)
    {
        RxPlayerWeaponMountPoint = new ReactiveProperty<Transform>();
    }

    public void Hit(EnemyModel enemyModel)
    {
        Player.WeaponHit(this, enemyModel);
    }
}
