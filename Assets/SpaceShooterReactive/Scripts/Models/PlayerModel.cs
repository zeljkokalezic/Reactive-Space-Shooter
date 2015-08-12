using UnityEngine;
using System.Collections;
using UniRx;

public class PlayerModel
{
    public ReactiveProperty<string> PlayerName { get; private set; }

    public PlayerModel()
    {
        PlayerName = new ReactiveProperty<string>(string.Empty);
    }

    internal void ChangeName(string p)
    {
        PlayerName.Value = p;
    }
}
