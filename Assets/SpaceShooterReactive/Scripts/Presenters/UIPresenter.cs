using UnityEngine;
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

    [Inject]
    private PlayerModel player;

    [Inject]
    private GameModel game;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(actionButton);
        Assert.IsNotNull(infoLabel);

        player.RxPlayerName.Subscribe(x => infoLabel.text = "Welcome " + x).AddTo(this);

        //player.RxPlayerFireRate.Subscribe(x => Debug.Log(x)).AddTo(this);

        actionButton.onClick.AsObservable().Subscribe(_ =>
        {
            infoLabel.enabled = false;
            actionButton.Visible(false);
            game.StartGame();
        }).AddTo(this);
    }

    
}
