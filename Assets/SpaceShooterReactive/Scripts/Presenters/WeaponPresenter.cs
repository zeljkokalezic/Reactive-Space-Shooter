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
        //event test
        Model.RxWeaponFiring
            .Where(x => x == true)
            .Subscribe(x => weaponBulletPresenterFactory.Create(Model.RxWeaponBullet.Value, Model)).AddTo(this);
    }
}
