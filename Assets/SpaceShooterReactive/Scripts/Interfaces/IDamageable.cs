using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

/// <summary>
/// Any game world object that can be damaged should implement this interface
/// </summary>
public interface IDamageable
{
    //use int, there can be precision problems with float
    ReactiveProperty<int> RxHealth { get; set; }
    ReactiveProperty<int> RxScore { get; set; }
    void HitByWeapon(WeaponModel weaponModel, IArmed other);
}

