using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class EnemyPresenter : MonoBehaviour
{
    public class Factory : PrefabFactory<EnemyModel, EnemyPresenter>
    {
    }

    [Serializable]
    public class Settings
    {

        public GameObject explosion;
        public GameObject healthBar;
    }
    public Transform shotSpawn;

    [Inject]
    private Settings settings;

    [Inject]
    public EnemyModel Model { get; private set; }

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    [Inject]
    protected readonly HealthBar.Factory healthBarFactory;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        var healthBar = healthBarFactory.Create(settings.healthBar, Model);
        healthBar.Hide();
        healthBar.transform.SetParent(this.gameObject.transform);

        //this.gameObject.OnTriggerEnterAsObservable()
        //    .Subscribe(other => {
        //    }).AddTo(this);

        Model.RxHealth
            .Where(x => x <= 0)
            .Subscribe(x => {
                Instantiate(settings.explosion, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject, 0.1f);
            }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(10))
           .Subscribe(x => {
               Destroy(this.gameObject);
           }).AddTo(this);

        componentFactory.Create<Damageable>(this.gameObject, Model);
        componentFactory.Create<DestroyOnGameOver>(this.gameObject);

        switch (Model.RxEnemyType.Value)
        {
            case EnemyModel.Type.Asteroid:
                break;
            case EnemyModel.Type.Ship:
                Model.EnemyWeapon.RxWeaponMountPoint.Value = shotSpawn;
                Model.EnemyWeapon.RxWeaponState.Value = WeaponModel.WeaponState.Active;
                componentFactory.Create<Mover>(this.gameObject, -5f);
                componentFactory.Create<ShipDriverAI>(this.gameObject);
                componentFactory.Create<WeaponPresenter>(this.gameObject, Model.EnemyWeapon);
                componentFactory.Create<WeaponTriggerTimer>(this.gameObject, Model.EnemyWeapon);
                break;
            default:
                break;
        }
    }
}
