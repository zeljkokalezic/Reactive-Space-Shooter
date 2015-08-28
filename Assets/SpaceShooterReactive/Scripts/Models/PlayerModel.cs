using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class PlayerModel
{
    [Serializable]
    public class Settings
    {
        public string playerName;
        public int speed;
        public int score;
        public float fireRate;
        public WeaponModel.Settings weaponSettings;//do we need this ? - maybe as a global weapon setting defaults ?
    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerSpeed { get; private set; }
    public ReactiveProperty<int> RxPlayerScore { get; private set; }
    public ReactiveProperty<float> RxPlayerFireRate { get; private set; }

    public WeaponModel PlayerWeapon { get; private set; }

    [Inject]
    public PlayerModel(Settings playerSettings, WeaponModel.Factory playerWeaponFactory)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerSpeed = new ReactiveProperty<int>(playerSettings.speed);
        RxPlayerScore = new ReactiveProperty<int>(playerSettings.score);
        //move this to weapon
        RxPlayerFireRate = new ReactiveProperty<float>(playerSettings.fireRate);
        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);

        //now we have ability to instantiate multiple weapons
        PlayerWeapon = playerWeaponFactory.Create(this, playerSettings.weaponSettings);
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
    }

    internal void WeaponHit(WeaponModel weaponModel, EnemyModel enemyModel)
    {
        RxPlayerScore.Value += enemyModel.RxEnemyScore.Value;
    }
}
