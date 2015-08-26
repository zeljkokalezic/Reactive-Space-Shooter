﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;

public class UIPresenter : MonoBehaviour
{

    public Button actionButton;
    public Text infoLabel;
    public Text scoreLabel;

    [Inject]
    private PlayerModel player;

    [Inject]
    private GameModel game;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(actionButton);
        Assert.IsNotNull(scoreLabel);
        Assert.IsNotNull(infoLabel);

        scoreLabel.enabled = false;
        player.RxPlayerScore.Subscribe(x => scoreLabel.text = "Score: " + x).AddTo(this);

        player.RxPlayerName.Subscribe(x => infoLabel.text = "Welcome " + x).AddTo(this);

        //player.RxPlayerFireRate.Subscribe(x => Debug.Log(x)).AddTo(this);

        actionButton.onClick.AsObservable().Subscribe(_ =>
        {
            scoreLabel.enabled = true;
            infoLabel.enabled = false;
            actionButton.Visible(false);
            game.StartGame();
        }).AddTo(this);
    }

    
}
