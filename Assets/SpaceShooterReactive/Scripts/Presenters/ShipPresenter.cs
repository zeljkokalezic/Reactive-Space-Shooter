using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class ShipPresenter : MonoBehaviour
{
    public class Factory : PrefabFactory<ShipModel, Settings, ShipPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        public GameObject shipExplosion;
    }

    public Transform[] shotSpawns;

    private Vector3 startPosition;

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    [Inject]
    private Settings settings;

    [Inject]
    public ShipModel Model { get; private set; }

    [Inject]
    private WeaponPresenter.Factory weaponPresenterFactory;

    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(Model);

        //transfer mount points and create weapons in a loop
        //create trigger for all weapons here (one is ok for now)
        for (int i = 0; i < Model.ShipWeapons.Length; i++)
        {
            Model.ShipWeapons[i].RxWeaponMountPoint.Value = shotSpawns[i];
            componentFactory.Create<WeaponPresenter>(this.gameObject, Model.ShipWeapons[i]);
            componentFactory.Create<WeaponTriggerButton>(this.gameObject, Model.ShipWeapons[i], "Fire1");
        }

        componentFactory.Create<ShipDriverPlayer>(this.gameObject, Model);
        componentFactory.Create<Damageable>(this.gameObject, Model);

        startPosition = this.transform.position;

        Model.RxShipState
            .Where(x => x == ShipModel.ShipState.Dead)
            .Subscribe(x =>
            {
                GetComponent<MeshCollider>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Instantiate(settings.shipExplosion, this.transform.position, this.transform.rotation);
                SetVisiblity(false);
            }).AddTo(this);

        Model.RxShipState
            .Where(x => x == ShipModel.ShipState.Active)
            .Subscribe(x =>
            {
                GetComponent<MeshCollider>().enabled = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.position = startPosition;
                SetVisiblity(true);
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

    void SetVisiblity(bool visible)
    {
        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}