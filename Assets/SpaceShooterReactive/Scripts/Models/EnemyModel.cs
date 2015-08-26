using UnityEngine;
using System.Collections;
using System;
using Zenject;
using UniRx;

public class EnemyModel
{
    [Serializable]
    public class Settings
    {
        public int score;
    }

    public ReactiveProperty<int> RxEnemyScore { get; private set; }

    [Inject]
    public EnemyModel(Settings enemySettings)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        RxEnemyScore = new ReactiveProperty<int>(enemySettings.score);      
    }

}
