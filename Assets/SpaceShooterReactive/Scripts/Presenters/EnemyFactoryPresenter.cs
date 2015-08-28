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

    public GameObject[] asteroids;

    [PostInject]
    void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
            .Subscribe(x =>
                {
                    var randomNumber = UnityEngine.Random.Range(0, asteroids.Length);
                    GameObject asteroid = asteroids[randomNumber];
                    //we can also create factory with parameters and pass settings for example, and/or enemy model
                    enemyPresenterFactory.Create(asteroid);
                })
            .AddTo(this);
    }

}
