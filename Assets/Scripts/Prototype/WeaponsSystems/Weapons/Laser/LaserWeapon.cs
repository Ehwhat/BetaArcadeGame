using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Weapons/New Laser Weapon", fileName = "New Laser Weapon")]
public class LaserWeapon : TankWeapon {

    private LaserChargingVisualisation chargingVisualisation;

    public override void OnWeaponEquipted(TankWeaponHolder holder)
    {
        chargingVisualisation = holder.GetCurrentWeaponVisualisation().GetComponentInChildren<LaserChargingVisualisation>();
        if (holder.ownerTank is PlayerTankManager)
        {
            chargingVisualisation.SetColour((holder.ownerTank as PlayerTankManager).data.playerColour);
        }
    }

    protected override bool OnCharge(Vector2 position, Vector2 direction, TankWeaponHolder holder, float chargeAmount)
    {
        return base.OnCharge(position, direction, holder, chargeAmount);
    }

    protected override bool FireProjectile(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        chargingVisualisation.OnCharge(0);
        return base.FireProjectile(position, direction, holder);
    }

}
