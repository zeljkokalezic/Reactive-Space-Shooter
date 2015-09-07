using UnityEngine;
using System.Collections;
using System;
using Zenject;
using UniRx;

public class EnemyModel : IArmed, IDamageable
{
    public enum Type { Asteroid, Ship }

    public class Factory : Factory<Settings, Type, EnemyModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public int score;
        public int health;
        public WeaponModel.Settings weaponSettings;
    }

    public ReactiveProperty<Type> RxEnemyType { get; private set; }

    //IDamageable
    public ReactiveProperty<int> RxHealth { get; set; }
    public ReactiveProperty<int> RxScore { get; set; }

    public WeaponModel EnemyWeapon { get; private set; }

    [Inject]
    public EnemyModel(Settings enemySettings, WeaponModel.Factory weaponFactory, Type type)
    {
        RxScore = new ReactiveProperty<int>(enemySettings.score);
        RxEnemyType = new ReactiveProperty<Type>(type);
        RxHealth = new ReactiveProperty<int>(enemySettings.health);

        //we need a better solution for enemy types (subclasses maybe), but let it be
        //also extract the player class properties to the new base class
        if (RxEnemyType.Value == Type.Ship)
        {
            EnemyWeapon = weaponFactory.Create(this, enemySettings.weaponSettings);
        }
    }


    public void HitByWeapon(WeaponModel weaponModel, IArmed other)
    {
        //set state to destroyed here if health == 0
        //process aditional options if needed (reduce health, etc..)
        //Debug.Log("Enemy Hit");
        //Debug.Log(weaponModel);
        //Debug.Log(other);
    }

    public void WeaponHit(WeaponModel weaponModel, IDamageable other)
    {
        //do nothing for now
    }
}
