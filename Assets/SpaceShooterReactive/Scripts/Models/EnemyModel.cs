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
        RxEnemyScore = new ReactiveProperty<int>(enemySettings.score);      
    }

}
