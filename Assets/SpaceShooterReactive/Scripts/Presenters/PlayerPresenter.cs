using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class PlayerPresenter : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
    }

    public Transform shotSpawn;

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    [Inject]
    private Settings settings;

    [Inject]
    private PlayerModel Model;

    [Inject]
    private WeaponPresenter.Factory weaponPresenterFactory;

    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(Model);

        Model.PlayerWeapon.RxWeaponMountPoint.Value = shotSpawn;
        componentFactory.Create<WeaponPresenter>(this.gameObject, Model.PlayerWeapon);
        componentFactory.Create<ShipDriverPlayer>(this.gameObject, Model);
        componentFactory.Create<WeaponTriggerButton>(this.gameObject, Model.PlayerWeapon, "Fire1");
        componentFactory.Create<Damageable>(this.gameObject, Model);
    }
}
