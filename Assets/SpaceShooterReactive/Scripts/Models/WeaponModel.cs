using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class WeaponModel
{
    public class Factory : Factory<IArmed, Settings, WeaponModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public GameObject bullet;
        public float fireRate;
    }

    public enum WeaponState { Inactive, Active }

    public ReactiveProperty<WeaponState> RxWeaponState { get; private set; }
    public ReactiveProperty<Transform> RxWeaponMountPoint { get; private set; }
    public ReactiveProperty<GameObject> RxWeaponBullet { get; private set; }
    public ReactiveProperty<float> RxWeaponFireRate { get; private set; }
    public ReactiveProperty<bool> RxWeaponFiring { get; private set; }

    [Inject]
    public IArmed WeaponOwner { get; set; }

    [Inject]
    public WeaponModel(Settings weaponSettings)
    {
        RxWeaponMountPoint = new ReactiveProperty<Transform>();
        RxWeaponBullet = new ReactiveProperty<GameObject>(weaponSettings.bullet);
        RxWeaponState = new ReactiveProperty<WeaponState>(WeaponState.Inactive);
        RxWeaponFireRate = new ReactiveProperty<float>(weaponSettings.fireRate);
        RxWeaponFiring = new ReactiveProperty<bool>(false);
    }

    public void Hit(IDamageable other)
    {
        WeaponOwner.WeaponHit(this, other);
    }
}
