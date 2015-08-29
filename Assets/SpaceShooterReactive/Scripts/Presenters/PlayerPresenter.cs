﻿using UnityEngine;
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
        public GameObject weaponPresenter;
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

        //for each player weapon create a weapon presenter
        //we have just one now
        Model.PlayerWeapon.RxPlayerWeaponMountPoint.Value = shotSpawn;
        componentFactory.Create<WeaponPresenter>(this.gameObject, Model.PlayerWeapon);
        //weaponPresenterFactory.Create(settings.weaponPresenter, Model.PlayerWeapon).gameObject.transform.parent = this.transform;

        //ship driver component
        componentFactory.Create<ShipDriverPlayer>(this.gameObject, Model);
        //componentFactory.Create<Mover>(this.gameObject, 1f);
        //omponentFactory.Create<ShipDriverAI>(this.gameObject);        
    }
}
