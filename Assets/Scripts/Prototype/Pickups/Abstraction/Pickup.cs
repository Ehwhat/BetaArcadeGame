using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Pickup : MonoBehaviour {

    public bool isEnabled = true;
    public float respawnDelay = 4;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnabled)
        {
            TankManager tank = collision.attachedRigidbody.GetComponent<TankManager>();
            if (tank)
            {
                OnPickup(tank);
                RespawnWait();
            }
        }
    }

    private IEnumerator RespawnWait()
    {
        isEnabled = false;
        yield return new WaitForSeconds(respawnDelay);
        isEnabled = true;
    }

    public abstract void OnPickup(TankManager tank);

}
