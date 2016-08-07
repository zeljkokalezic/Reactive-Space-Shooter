using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class PlayerModel: IArmed, IDamageable, IShipOwner
{
    [Serializable]
    public class Settings
    {
        public string playerName;
        public int initialScore;
        public ShipModel.Settings shipSettings;

    }

    public enum PlayerState { Inactive, Active, Dead }

    public ReactiveProperty<PlayerState> RxPlayerState { get; private set; }
    public ReactiveProperty<string> RxPlayerName { get; private set; }
    public ReactiveProperty<int> RxPlayerScore { get; private set; }

    public ReactiveProperty<int> RxHealth { get; set; }
    public ReactiveProperty<int> RxScore { get; set; }

    public ShipModel PlayerShip { get; private set; }

    public Settings ModelSettings { get; private set; }


    [Inject]
    public PlayerModel(Settings playerSettings, ShipModel.Factory shipFactory)
    {
        //order of initilaization is based on object graph, if object A is injected into B A is initalized first !

        ModelSettings = playerSettings;

        RxPlayerName = new ReactiveProperty<string>(playerSettings.playerName);        
        RxPlayerScore = new ReactiveProperty<int>(playerSettings.initialScore);
        RxPlayerState = new ReactiveProperty<PlayerState>(PlayerState.Inactive);

        PlayerShip = shipFactory.Create(this, playerSettings.shipSettings);
    }

    internal void ChangeName(string p)
    {
        RxPlayerName.Value = p;
    }

    internal void Activate()
    {
        RxPlayerState.Value = PlayerState.Active;
        RxPlayerScore.Value = 0;
        //Activate the ship
        //Do this trough ShipOwner interface later (decoupling) ?
        PlayerShip.Activate();
    }

    internal void Deactivate()
    {
        RxPlayerState.Value = PlayerState.Dead;
        //should the ship deactivate itself ?
        PlayerShip.Deactivate();
    }

    public void HitByWeapon(WeaponModel weaponModel, IArmed other)
    {
        Deactivate();
    }

    public void WeaponHit(WeaponModel weaponModel, IDamageable other)
    {
        //should the interface hold the health or just state (dead/alive) ?
        if (other.RxHealth.Value == 0)//if the enemy is dead increase the score
        {
            RxPlayerScore.Value += other.RxScore.Value;
        }
    }
}
