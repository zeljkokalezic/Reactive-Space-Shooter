using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

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

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        this.transform.position = Model.RxWeaponMountPoint.Value.position;
        this.transform.rotation = Model.RxWeaponMountPoint.Value.rotation;

        GetComponent<Rigidbody>().velocity = transform.forward * 20;//this is hypotetical bullet(weapon speed)

        this.gameObject.OnTriggerEnterAsObservable()
            .Subscribe(other =>
            {
                var enemy = other.GetComponent<Damageable>();
                if (enemy != null)
                {
                    Model.Hit(enemy.Model);
                }

                if (Model.WeaponOwner.GetType() == enemy.Model.GetType())
                {

                }
                else
                {
                    Destroy(this.gameObject);
                }
            }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(5))
           .Subscribe(x =>
           {
               Destroy(this.gameObject);
           }).AddTo(this);
    }
}
