using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using System;
using UniRx.Triggers;

public class WeaponPresenter : MonoBehaviour
{

    public class Factory : PrefabFactory<WeaponModel, WeaponPresenter>
    {
    }

    [Inject]
    private WeaponModel Model;

    [Inject]
    private WeaponBulletPresenter.Factory weaponBulletPresenterFactory;

    private float nextFire;

    [PostInject]
    void Initialize()
    {
        //rework this into fire command that is sent to the model and the presenter responds
        //fire command (property) is observable and is initiated by the trigger
        this.gameObject.AddComponent<ObservableUpdateTrigger>()
            .UpdateAsObservable()
            .Where(_ => Model.RxWeaponState.Value == WeaponModel.WeaponState.Active
                && Model.RxWeaponFiring.Value
                && Time.time > nextFire)
            .Subscribe(x =>
            {
                //Debug.Log(Model.WeaponOwner + " Firing");
                weaponBulletPresenterFactory.Create(Model.RxWeaponBullet.Value, Model);
                nextFire = Time.time + Model.RxWeaponFireRate.Value;
            });
    }
}
