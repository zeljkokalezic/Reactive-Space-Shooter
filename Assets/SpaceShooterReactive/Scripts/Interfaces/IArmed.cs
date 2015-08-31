using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Any game world object with weapon should implement this interface
/// </summary>
public interface IArmed
{
    void WeaponHit(WeaponModel weaponModel, IDamageable other);
}

