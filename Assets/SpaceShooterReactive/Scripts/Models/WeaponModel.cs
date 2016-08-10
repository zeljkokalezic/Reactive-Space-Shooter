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
        public float bulletSpeed;
        //this can be separate damage descriptor with damage tables (armor x, shiled x, hull x, etc...)
        public int damage;
    }

    public enum WeaponState { Inactive, Active }

    public ReactiveProperty<WeaponState> RxWeaponState { get; private set; }
    public ReactiveProperty<Transform> RxWeaponMountPoint { get; private set; }
    public ReactiveProperty<GameObject> RxWeaponBullet { get; private set; }
    public ReactiveProperty<float> RxWeaponFireRate { get; private set; }
    public ReactiveProperty<float> RxWeaponBulletSpeed { get; private set; }
    public ReactiveProperty<int> RxWeaponDamage { get; private set; }
    public ReactiveProperty<bool> RxWeaponFiring { get; private set; }

    private float nextFireTime;

    [Inject]
    public IArmed WeaponOwner { get; set; }

    [Inject]
    public WeaponModel(Settings weaponSettings)
    {
        RxWeaponMountPoint = new ReactiveProperty<Transform>();
        RxWeaponBullet = new ReactiveProperty<GameObject>(weaponSettings.bullet);
        RxWeaponState = new ReactiveProperty<WeaponState>(WeaponState.Inactive);
        RxWeaponFireRate = new ReactiveProperty<float>(weaponSettings.fireRate);
        RxWeaponBulletSpeed = new ReactiveProperty<float>(weaponSettings.bulletSpeed);
        RxWeaponDamage = new ReactiveProperty<int>(weaponSettings.damage);
        RxWeaponFiring = new ReactiveProperty<bool>(false);
    }

    //we can make one with parameters for targeting weapons
    public void Fire()
    {
        //we can also process aditional stuff like bullet count, etc.. in the future

        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + RxWeaponFireRate.Value;
            //we can have parameters here not just simple bool (basically weapon bullet model)
            //that will describe things like bullet speed and fire angle for example
            RxWeaponFiring.SetValueAndForceNotify(true);
        }        
    }

    public void Hit(IDamageable other)
    {
        WeaponOwner.WeaponHit(this, other);
    }
}
