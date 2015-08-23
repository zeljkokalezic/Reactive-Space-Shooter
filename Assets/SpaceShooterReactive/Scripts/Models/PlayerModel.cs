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
        public float fireRate;
    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerSpeed { get; private set; }
    public ReactiveProperty<float> RxPlayerFireRate { get; private set; }

    [Inject]
    public PlayerModel(Settings playerSettings)
    {
        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerSpeed = new ReactiveProperty<int>(playerSettings.speed);
        RxPlayerFireRate = new ReactiveProperty<float>(playerSettings.fireRate);

        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
    }
}
