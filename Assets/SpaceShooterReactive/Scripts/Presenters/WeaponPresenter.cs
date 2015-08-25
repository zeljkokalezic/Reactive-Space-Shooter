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
    public class Factory : GameObjectFactory<WeaponPresenter.Settings, WeaponPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        //move to weapon model ?
        public Transform mountPosition;
    }

    [Inject]
    private Settings settings;

    //[Inject]
    //private PlayerModel player;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()//Settings settings, PlayerModel player)
    {
        //should the spawner set the enemy position, or set the value in tne enemy model and then the presenter reads from there ?
        this.transform.position = 
            //new Vector3(UnityEngine.Random.Range(-settings.mountPosition.position.x, settings.mountPosition.position.x), settings.mountPosition.position.y, settings.mountPosition.position.z);
            settings.mountPosition.position;

        this.gameObject.AddComponent<ObservableCollisionTrigger>()
                .OnTriggerEnterAsObservable()
                .Subscribe(x =>
                {
                    //if (x.GetComponent<Done_DestroyByBoundary>() != null)
                    //{
                    //    return;
                    //}

                    Destroy(x.gameObject);
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
