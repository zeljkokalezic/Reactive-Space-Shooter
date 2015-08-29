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
        this.gameObject.AddComponent<ObservableUpdateTrigger>()
            .UpdateAsObservable()
            .Where(_ => Model.Player.RxPlayerState.Value == PlayerModel.PlayerState.Active
                && Input.GetButton("Fire1")
                && Time.time > nextFire)//next fire should be a weapon property
            .Subscribe(x =>
            {
                weaponBulletPresenterFactory.Create(Model.RxPlayerWeaponBullet.Value, Model);
                nextFire = Time.time + Model.Player.RxPlayerFireRate.Value;
            });
    }
}
