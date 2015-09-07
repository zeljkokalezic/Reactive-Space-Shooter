using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class GameModel 
{
    [Serializable]
    public class Settings
    {
     
    }

    public enum GameState { WaitingToStart, InProgress, GameOver }

    private PlayerModel _player;

    public ReactiveProperty<GameState> RxGameState { get; private set; }

    [Inject]
    public GameModel(PlayerModel player)
    {
        _player = player;
        RxGameState = new ReactiveProperty<GameState>(GameState.WaitingToStart);
        _player.RxPlayerState
            .Where(x => x == PlayerModel.PlayerState.Dead)
            .Subscribe(x =>
            {
                RxGameState.Value = GameState.GameOver;
            });
    }

    public void StartGame()
    {
        RxGameState.Value = GameState.InProgress;
        _player.Activate();
    }
}
