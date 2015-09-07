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
        public GameObject playerExplosion;
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

        Model.RxPlayerState
            .Where(x => x == PlayerModel.PlayerState.Dead)
            .Subscribe(x =>
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Instantiate(settings.playerExplosion, this.transform.position, this.transform.rotation);
                ToggleVisibility();//TODO: disable the colider also
            }).AddTo(this);

        //should we move the colison handling to the damageable component ?
        this.gameObject.OnTriggerEnterAsObservable()
            .Subscribe(other =>
            {
                var enemy = other.GetComponent<Damageable>();
                if (enemy != null)
                {
                    Model.Deactivate();
                }
            }).AddTo(this);
    }

    void ToggleVisibility()
    {
        // toggles the visibility of this gameobject and all it's children
        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = !r.enabled;
        }
    }
}
