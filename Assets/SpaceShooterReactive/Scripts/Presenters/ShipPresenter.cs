using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;
using UnityEngine.Assertions;
using UniRx.Triggers;

public class ShipPresenter : MonoBehaviour
{
    public class Factory : PrefabFactory<ShipModel, Settings, ShipPresenter>
    {
    }

    [Serializable]
    public class Settings
    {
    }

    [Inject]
    public ShipModel Model { get; private set; }

    [Inject]
    private Settings settings;

    [Inject]
    protected readonly SimpleComponentFactory componentFactory;

    //we can create shoot spawn component and then use get component in childern to get shoot spawns
    //shoot spawn component can have a wepon id to bind to apropriate weapon
    public Transform[] shotSpawns;

    // Use this for initialization
    [PostInject]
    void InitializePresenter()
    {

    }
}