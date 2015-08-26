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
    public class Factory : PrefabFactory<EnemyPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        //set transform here
        //move this to the enemy, or to the spawner ?
        public Vector3 spawnPosition;
        public GameObject explosion;
    }

    [Inject]
    private Settings settings;

    //no need for a player here
    //[Inject]
    //private PlayerModel player;

    //[InjectOptional]
    //private int test;

    //[Inject]
    //private GameModel game;

    [Inject]
    public EnemyModel Model { get; private set; }

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        //should the spawner set the enemy position, or set the value in tne enemy model and then the presenter reads from there <- THIS
        this.transform.position = new Vector3(UnityEngine.Random.Range(-settings.spawnPosition.x, settings.spawnPosition.x), settings.spawnPosition.y, settings.spawnPosition.z);

        this.gameObject//.AddComponent<ObservableCollisionTrigger>() //-> WARNING: this is a colision triger not trigger triger
                .OnTriggerEnterAsObservable() //this will add required component automaticaly
            //.Where(x => x.gameObject.GetComponent<WeaponPresenter>() != null)
                .Subscribe(other =>
                {
                    ////we should increase player score trough the weapon model to suport hypotetical multiplayer
                    ////should the weapon increase the score or the enemy ? -> Weapon
                    //var weaponPresenter = other.GetComponent<WeaponPresenter>();
                    //if (weaponPresenter != null)
                    //{
                    //    //this should be a hit function that takes the enemy as variable
                    //    weaponPresenter.Model.Player.RxPlayerScore.Value += Model.RxEnemyScore.Value;
                    //}

                    //Debug.Log(this);
                    //Debug.Log(other);

                    Instantiate(settings.explosion, other.transform.position, other.transform.rotation);
                    Destroy(this.gameObject);
                }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(10))
           .Subscribe(x =>
           {
               Destroy(this.gameObject);
           })
           .AddTo(this);

        //Debug.Log(test);
    }
}
