using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class WeaponPresenter : MonoBehaviour
{
    public class Factory : PrefabFactory<WeaponModel, WeaponPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        //move to weapon model ? -> YES
        //public Transform mountPosition;
    }

    //[Inject]
    //private Settings settings;

    //[Inject]
    //private PlayerModel player;

    [Inject]
    public WeaponModel Model { get; private set; }

    // Use this for initialization
    [PostInject]
    void InitializePresenter()//Settings settings, PlayerModel player)
    {
        //should the spawner set the enemy position, or set the value in tne enemy model and then the presenter reads from there ?
        this.transform.position =
            //new Vector3(UnityEngine.Random.Range(-settings.mountPosition.position.x, settings.mountPosition.position.x), settings.mountPosition.position.y, settings.mountPosition.position.z);
            //settings.mountPosition.position;
            Model.RxPlayerWeaponMountPoint.Value.position;

        this.gameObject//.AddComponent<ObservableTriggerTrigger>()
                .OnTriggerEnterAsObservable()
            //this can be implemented as extension OnTriggerEnterAsObservableWith<T> 
            //http://chaoscultgames.com/2014/03/unity3d-mythbusting-performance/
                //.Where(x => x.gameObject.GetComponent<EnemyPresenter>() != null)
                .Subscribe(other =>
                {
                    ////we should increase player score trough the weapon model to suport hypotetical multiplayer
                    ////should the weapon increase the score or the enemy ? -> Weapon
                    var enemyPresenter = other.GetComponent<EnemyPresenter>();
                    if (enemyPresenter != null)
                    {
                        //this should be a hit function that takes the enemy as variable
                        //enemyPresenter.Model.Player.RxPlayerScore.Value += Model.RxEnemyScore.Value;
                        Model.Hit(enemyPresenter.Model);
                    }

                    //Debug.Log(this);
                    //Debug.Log(x);

                    //if (x.GetComponent<WeaponPresenter>() != null)
                    //{
                    //    return;
                    //}

                    //premature optimization !
                    ////above also works, this should be faster
                    //if (x.gameObject.name == this.gameObject.name)
                    //{
                    //    return;
                    //}

                    //who should detroy who ?
                    //everybody responsible for his destruction ? <- YES
                    //Destroy(x.gameObject);
                    Destroy(this.gameObject);
                }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(10))
           .Subscribe(x =>
           {
               Destroy(this.gameObject);
           }).AddTo(this);
    }
}
