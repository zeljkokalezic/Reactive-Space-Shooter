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

    [Inject]
    private IInstantiator gameObjectCreator;

    public GameObject[] asteroids;

    [PostInject]
    void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(_ => game.RxGameState.Value == GameModel.GameState.InProgress)
            .Subscribe(x =>
                {
                    //this will create the enemy with the default prefab, we want diferent prefabs
                    //enemyPresenterFactory.Create();
                    var randomNumber = UnityEngine.Random.Range(0, asteroids.Length);
                    GameObject asteroid = asteroids[randomNumber];
                    //var enemy = gameObjectCreator.Instantiate<EnemyModel>();
                    //gameObjectCreator.InstantiatePrefabForComponent<EnemyPresenter>(asteroid
                    //    //, new EnemyPresenter.Settings() //this will replace the globaly registered instance
                    //    //, randomNumber
                    //    );
                    //new implementation
                    enemyPresenterFactory.Create(asteroid);
                    //enemyPresenterFactory.Create((GameObject)null);//asteroid);
                    //enemyPresenterFactory.Create("Asteroid Test 1");
                })
            .AddTo(this);
    }

}
