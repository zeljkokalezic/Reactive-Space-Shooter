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
    public class Factory : GameObjectFactory<EnemyPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
        public Vector3 spawnPosition;
    }

    [Inject]
    private Settings settings;

    [Inject]
    private PlayerModel player;

    //[Inject]
    //private GameModel game;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        this.transform.position = new Vector3(UnityEngine.Random.Range(-settings.spawnPosition.x, settings.spawnPosition.x), settings.spawnPosition.y, settings.spawnPosition.z);

        this.gameObject.AddComponent<ObservableCollisionTrigger>()
                .OnTriggerEnterAsObservable()
                .Subscribe(x =>
                {
                    ////Debug.Log(x);
                    //if (x.GetComponent<Done_DestroyByBoundary>() != null)
                    //{
                    //    return;
                    //}
                    //for test only
                    //player.RxPlayerFireRate.Value -= 0.05f;
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
