﻿using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using System;

public class EnemyFactoryPresenter : MonoBehaviour
{

    [Inject]
    private GameModel game;

    [Inject]
    private EnemyPresenter.Factory enemyPresenterFactory;

    [Inject]
    private EnemyModel.Factory enemyModelFactory;

    [Inject]
    private EnemyModel.Settings enemyModelDefaultSettings;

    private double spawnRate = 3;
    public Vector3 spawnPosition;

    public GameObject[] asteroids;

    public GameObject[] ships;

    [PostInject]
    void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(spawnRate))
            .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
            .Subscribe(x =>
                {
                    var enemyType = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EnemyModel.Type)).Length);
                    var model = enemyModelFactory.Create(enemyModelDefaultSettings, (EnemyModel.Type)enemyType);
                    switch (model.RxEnemyType.Value)
                    {
                        case EnemyModel.Type.Asteroid:
                            CreateEnemy(model, asteroids);
                            break;
                        case EnemyModel.Type.Ship:
                            CreateEnemy(model, ships);
                            break;
                        default:
                            break;
                    }
                    
                })
            .AddTo(this);
    }

    private void CreateEnemy(EnemyModel model, GameObject[] prefabs)
    {
        var randomNumber = UnityEngine.Random.Range(0, prefabs.Length);
        var enemyPresenter = enemyPresenterFactory.Create(prefabs[randomNumber], model);
        enemyPresenter.transform.position = new Vector3(UnityEngine.Random.Range(-spawnPosition.x, spawnPosition.x), spawnPosition.y, spawnPosition.z);
    }
}
