using UnityEngine;
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

    public GameObject[] asteroids;

    public GameObject[] ships;

    [PostInject]
    void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
            .Subscribe(x =>
                {
                    //var randomNumber = UnityEngine.Random.Range(0, asteroids.Length);
                    //GameObject asteroid = asteroids[randomNumber];
                    //we can also create factory with parameters and pass settings for example, and/or enemy model
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
        enemyPresenterFactory.Create(prefabs[randomNumber], model);
    }
}
