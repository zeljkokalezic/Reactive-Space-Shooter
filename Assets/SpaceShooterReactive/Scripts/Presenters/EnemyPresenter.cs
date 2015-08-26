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

    [Inject]
    public EnemyModel Model { get; private set; }

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        //should the spawner set the enemy position, or set the value in tne enemy model and then the presenter reads from there <- THIS
        this.transform.position = new Vector3(UnityEngine.Random.Range(-settings.spawnPosition.x, settings.spawnPosition.x), settings.spawnPosition.y, settings.spawnPosition.z);

        this.gameObject.OnTriggerEnterAsObservable() //this will add required component automaticaly
            //.Where(x => x.gameObject.GetComponent<WeaponPresenter>() != null)
            .Subscribe(other =>
            {
                Instantiate(settings.explosion, other.transform.position, other.transform.rotation);
                Destroy(this.gameObject);
            }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(10))
           .Subscribe(x =>
           {
               Destroy(this.gameObject);
           })
           .AddTo(this);
    }
}
