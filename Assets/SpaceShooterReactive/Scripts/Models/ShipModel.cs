using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class ShipModel : IArmed, IDamageable
{
    public enum ShipState { Inactive, Active, Dead }

    //NO !!!
    //Use inheritance/composition
    public enum ShipType { Player, AI }

    public class Factory : Factory<IShipOwner, Settings, ShipModel>
    {
    }

    [Serializable]

    public class Settings
    {
        //We should extract IScorable later
        //why does ship has a score ?
        public int score; //move this to a component ?
        public int health;
        public int speed;
        public GameObject shipPrefab;
        public ShipPresenter.Settings shipPresenterSettings;
        public WeaponModel.Settings[] weaponSettings;
    }

    public ReactiveProperty<ShipState> RxShipState { get; private set; }
    public ReactiveProperty<int> RxShipSpeed { get; private set; }

    //IDamageable
    public ReactiveProperty<int> RxHealth { get; set; }
    public ReactiveProperty<int> RxScore { get; set; }

    //On ship owner deactivation deactivate the ship !
    [Inject]
    public IShipOwner ShipOwner { get; private set; }

    public WeaponModel[] ShipWeapons { get; private set; }

    [Inject]
    public ShipModel(Settings settings, WeaponModel.Factory weaponFactory)
    {
        RxShipState = new ReactiveProperty<ShipState>(ShipState.Inactive);
        RxScore = new ReactiveProperty<int>(settings.score);
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
        for (int i = 0; i < ShipWeapons.Length; i++)
        {
            ShipWeapons[i].RxWeaponState.Value = WeaponModel.WeaponState.Active;
        }
    }

    internal void Deactivate()
    {
        RxShipState.Value = ShipState.Dead;
        for (int i = 0; i < ShipWeapons.Length; i++)
        {
            ShipWeapons[i].RxWeaponState.Value = WeaponModel.WeaponState.Inactive;
        }
    }

    public void WeaponHit(WeaponModel weaponModel, IDamageable other)
    {
        //report hit to owner also
        (ShipOwner as IArmed).WeaponHit(weaponModel, other);
    }

    public void HitByWeapon(WeaponModel weaponModel, IArmed other)
    {
        //we should deduct weapon damage here
        if (RxHealth.Value == 0)
        {
            Debug.Log(weaponModel);
            Deactivate();
        }
        (ShipOwner as IDamageable).HitByWeapon(weaponModel, other);
    }
}