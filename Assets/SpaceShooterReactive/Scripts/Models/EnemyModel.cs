using UnityEngine;
using System.Collections;
using System;
using Zenject;
using UniRx;

public class EnemyModel
{
    public enum Type { Asteroid, Ship }

    public class Factory : Factory<Settings, EnemyModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public Type type;
        public int score;
    }

    public ReactiveProperty<int> RxEnemyScore { get; private set; }
    public ReactiveProperty<Type> RxEnemyType { get; private set; }

    [Inject]
    public EnemyModel(Settings enemySettings)
    {
        RxEnemyScore = new ReactiveProperty<int>(enemySettings.score);
        RxEnemyType = new ReactiveProperty<Type>(enemySettings.type);
    }

}
