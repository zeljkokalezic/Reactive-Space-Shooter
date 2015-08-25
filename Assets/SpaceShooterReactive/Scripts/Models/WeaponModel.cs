using UnityEngine;
using System.Collections;
using UniRx;
using System;
using Zenject;

public class WeaponModel
{
    [Serializable]
    public class Settings
    {
        public Transform weaponMountPoint;
    }

    public ReactiveProperty<Transform> RxPlayerWeaponMountPoint { get; private set; }

    [Inject]
    public WeaponModel(Settings playerSettings)
    {
        RxPlayerWeaponMountPoint = new ReactiveProperty<Transform>(playerSettings.weaponMountPoint);
    }
}
