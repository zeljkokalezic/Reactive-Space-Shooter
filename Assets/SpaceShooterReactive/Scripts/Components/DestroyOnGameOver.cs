using UnityEngine;
using System.Collections;
using Zenject;
using UniRx;

public class DestroyOnGameOver : MonoBehaviour
{
    [Inject]
    private GameModel game;

    [PostInject]
    void InitializeComponent()
    {
        game.RxGameState
            .Where(x => x == GameModel.GameState.GameOver)
            .Subscribe(x => {
                Destroy(this.gameObject);
            }).AddTo(this);
    }
}