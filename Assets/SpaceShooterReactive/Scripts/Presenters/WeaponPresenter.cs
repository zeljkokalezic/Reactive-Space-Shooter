﻿using UnityEngine;
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

    }

    [Inject]
    public WeaponModel Model { get; private set; }

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        this.transform.position = Model.RxPlayerWeaponMountPoint.Value.position;

        this.gameObject
            .OnTriggerEnterAsObservable()
            //this can be implemented as extension OnTriggerEnterAsObservableWith<T> 
            //http://chaoscultgames.com/2014/03/unity3d-mythbusting-performance/
            //.Where(x => x.gameObject.GetComponent<EnemyPresenter>() != null)
            .Subscribe(other =>
            {
                var enemyPresenter = other.GetComponent<EnemyPresenter>();
                if (enemyPresenter != null)
                {
                    Model.Hit(enemyPresenter.Model);
                }
                Destroy(this.gameObject);
            }).AddTo(this);

        Observable.Interval(TimeSpan.FromSeconds(10))
           .Subscribe(x =>
           {
               Destroy(this.gameObject);
           }).AddTo(this);
    }
}
