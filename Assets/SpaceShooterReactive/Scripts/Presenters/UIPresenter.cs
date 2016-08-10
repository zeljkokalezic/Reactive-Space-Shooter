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
    public Text actionButtonText;
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

        //TODO: extract strings to config file later
        actionButtonText.text = "Start Game";
        scoreLabel.enabled = false;

        player.RxPlayerScore.Subscribe(x => scoreLabel.text = "Score: " + x).AddTo(this);
        player.RxPlayerName.Subscribe(x => infoLabel.text = "Welcome " + x).AddTo(this);

        game.RxGameState
            .Where(x => x == GameModel.GameState.GameOver)
            .Subscribe(x => {
                actionButtonText.text = "Restart";
                infoLabel.text = "Game Over";
                infoLabel.enabled = true;
                actionButton.Visible(true);
            }).AddTo(this);

        actionButton.onClick.AsObservable().Subscribe(_ => {
            //game state should trigger these UI changes not button click
            scoreLabel.enabled = true;
            infoLabel.enabled = false;
            actionButton.Visible(false);

            game.StartGame();
        }).AddTo(this);
    }

    
}
