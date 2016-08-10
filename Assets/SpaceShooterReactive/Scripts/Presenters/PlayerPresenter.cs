using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class PlayerPresenter : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        
    }

    [Inject]
    protected readonly ShipPresenter.Factory shipFactory;

    [Inject]
    private Settings settings;

    [Inject]
    private PlayerModel Model;

    private ShipPresenter playerShip;

    [PostInject]
    void InitializePresenter()
    {
        Assert.IsNotNull(Model);

        playerShip = shipFactory.Create(Model.ModelSettings.shipSettings.shipPrefab, Model.PlayerShip, Model.ModelSettings.shipSettings.shipPresenterSettings);
    }
}
