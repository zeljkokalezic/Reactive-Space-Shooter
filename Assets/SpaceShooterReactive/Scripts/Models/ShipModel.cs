using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class ShipModel : IArmed, IDamageable
{
    public enum ShipState { Inactive, Active }
    public enum ShipType { Player, AI }

    public class Factory : Factory<IShipOwner, Settings, ShipModel>
    {
    }

    [Serializable]
    public class Settings
    {
        public int score; //move this to a component ?
        public int health;
        public int speed;
        public ShipType shipType;
        public WeaponModel.Settings[] weaponSettings;
    }

    public ReactiveProperty<ShipState> RxShipState { get; private set; }
    public ReactiveProperty<ShipType> RxShipType { get; private set; }
    public ReactiveProperty<int> RxShipSpeed { get; private set; }

    //IDamageable
    public ReactiveProperty<int> RxHealth { get; set; }
    public ReactiveProperty<int> RxScore { get; set; }

    [Inject]
    public IShipOwner ShipOwner { get; private set; }

    public WeaponModel[] ShipWeapons { get; private set; }

    [Inject]
    public ShipModel(Settings settings, WeaponModel.Factory weaponFactory)
    {
        RxShipState = new ReactiveProperty<ShipState>(ShipState.Inactive);
        RxShipType = new ReactiveProperty<ShipType>(settings.shipType);
        RxScore = new ReactiveProperty<int>(settings.score);//to we need to extract this into IScorable for example ?
        RxHealth = new ReactiveProperty<int>(settings.health);
        RxShipSpeed = new ReactiveProperty<int>(settings.speed);

        //Multiple weapons support
        ShipWeapons = new WeaponModel[settings.weaponSettings.Length];
        for (int i = 0; i < settings.weaponSettings.Length; i++)
        {
            ShipWeapons[i] = weaponFactory.Create(this, settings.weaponSettings[i]);
        }        
    }

    internal void Activate()
    {
        RxShipState.Value = ShipState.Active;
    }

    internal void Deactivate()
    {
        RxShipState.Value = ShipState.Inactive;
    }

    public void WeaponHit(WeaponModel weaponModel, IDamageable other)
    {
        
    }

    public void HitByWeapon(WeaponModel weaponModel, IArmed other)
    {
        if (RxHealth.Value == 0)
        {
            Deactivate();
        }        
    }
}