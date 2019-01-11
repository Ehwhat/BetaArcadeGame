using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Weapons/New Laser Weapon", fileName = "New Laser Weapon")]
public class LaserWeapon : AutoAimTankWeapon {

    private LaserChargingVisualisation[] chargingVisualisations;

    public override void OnWeaponEquipted(TankWeaponHolder holder)
    {
        chargingVisualisations = holder.GetCurrentWeaponVisualisation().GetComponentsInChildren<LaserChargingVisualisation>();
        if (holder.ownerTank is PlayerTankManager)
        {
            for (int i = 0; i < chargingVisualisations.Length; i++)
            {
                chargingVisualisations[i].SetColour((holder.ownerTank as PlayerTankManager).colour);
                chargingVisualisations[i].OnCharge(0);
            }
            
        }
        
    }

    protected override bool OnCharge(Vector2 position, Vector2 direction, TankWeaponHolder holder, float chargeAmount)
    {
        chargingVisualisations = holder.GetCurrentWeaponVisualisation().GetComponentsInChildren<LaserChargingVisualisation>();
        for (int i = 0; i < chargingVisualisations.Length; i++)
        {
            chargingVisualisations[i].OnCharge(chargeAmount);
        }
        return base.OnCharge(position, direction, holder, chargeAmount);
    }

    protected override bool FireProjectile(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        chargingVisualisations = holder.GetCurrentWeaponVisualisation().GetComponentsInChildren<LaserChargingVisualisation>();
        for (int i = 0; i < chargingVisualisations.Length; i++)
        {
            chargingVisualisations[i].OnCharge(0);
        }
        return base.FireProjectile(position, direction, holder);
    }

}
