using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;
using System;

public class EnemySpawner : MonoBehaviour {

    [Inject]
    private PlayerModel player;

    [Inject]
    private GameModel game;

    [Inject]
    private EnemyPresenter.Factory enemyPresenterFactory;

    [PostInject]
    void Initialize()
    {
       //var enemyPresenter = enemyPresenterFactory.Create();

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
            .Subscribe(x =>
                {
                    enemyPresenterFactory.Create();
                })
            .AddTo(this);
    }

}
