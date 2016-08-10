using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

//should we make the bullets "dumb" (handle colisions in target with a componenet not in bullet) ?
public class WeaponBulletPresenter : MonoBehaviour
{
    public class Factory : PrefabFactory<WeaponModel, WeaponBulletPresenter>
    {
    }

    [Serializable]
    public class Settings
    {

    }

    [Inject]
    public WeaponModel Model { get; private set; }

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        this.transform.position = Model.RxWeaponMountPoint.Value.position;
        this.transform.rotation = Model.RxWeaponMountPoint.Value.rotation;

        //crete a bullet model or not ?
        GetComponent<Rigidbody>().velocity = transform.forward * Model.RxWeaponBulletSpeed.Value;

        this.gameObject.OnTriggerEnterAsObservable()
            .Subscribe(other => {
                var enemy = other.GetComponent<Damageable>();
                if (enemy != null)
                {
                    if (enemy.Model.RxHealth.Value > 0)
                    {
                        //first hit by weapon to deduct damage
                        enemy.Model.HitByWeapon(Model, Model.WeaponOwner);
                        //then signal hit
                        Model.Hit(enemy.Model);
                    }
                }

                //actors of same type does not hit each other (enemy-enemy, bullet-bullet)
                if ((enemy != null && Model.WeaponOwner.GetType() == enemy.Model.GetType()) || other.GetComponent<WeaponBulletPresenter>() != null)
                {

                }
                else
                {
                    Destroy(this.gameObject);
                }
            }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(5))
           .Subscribe(x => {
               Destroy(this.gameObject);
           }).AddTo(this);

        componentFactory.Create<DestroyOnGameOver>(this.gameObject);
    }
}
