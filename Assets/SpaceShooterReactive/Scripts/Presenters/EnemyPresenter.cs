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
    //pass the enemy model in future interation
    public class Factory : PrefabFactory<EnemyModel, EnemyPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        public Vector3 spawnPosition;
        public GameObject explosion;
    }
    public Transform shotSpawn;

    [Inject]
    private Settings settings;

    [Inject]
    public EnemyModel Model { get; private set; }

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        //should the spawner set the enemy position, or set the value in tne enemy model and then the presenter reads from there ?
        this.transform.position = new Vector3(UnityEngine.Random.Range(-settings.spawnPosition.x, settings.spawnPosition.x), settings.spawnPosition.y, settings.spawnPosition.z);

        this.gameObject.OnTriggerEnterAsObservable()
            .Subscribe(other => {
                var bullet = other.GetComponent<WeaponBulletPresenter>();
                if (bullet != null && bullet.Model.WeaponOwner.GetType() == Model.GetType())//disable frendly fire
                {
                    
                }
                else
                {
                    Instantiate(settings.explosion, other.transform.position, other.transform.rotation);
                    Destroy(this.gameObject);
                }
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
                //Model.EnemyWeapon.RxWeaponFiring.Value = true;
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
