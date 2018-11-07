using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmourPickupManager : MonoBehaviour {

    public TankArmourPickup pickupPrefab;
    public TankArmourManager manager;
    public float pickupRadius = 4;
    public float minSpeed = 1f;
    public float maxSpeed = 8f;

    public void Start()
    {
    }

    public void Update()
    {
        if (GameInput.GetPlayerDevice(0).RightBumper.IsPressed)
        {
            AttractArmourPickups();
        }
        if (GameInput.GetPlayerDevice(0).LeftBumper.WasPressed)
        {
            EjectArmourPickups();
        }
    }

    public void AttractArmourPickups()
    {
        List<TankArmourPiece> piecesReserved = new List<TankArmourPiece>();

        List<TankArmourPickup> pickupsDetected = DetectArmourPickups();
        for (int i = 0; i < pickupsDetected.Count; i++)
        {
            Vector2 direction = (pickupsDetected[i].transform.position - transform.position).normalized;
            TankArmourPiece piece = manager.ReservePieceToward(direction, true);
            piecesReserved.Add(piece);

            float distance = Vector2.Distance(pickupsDetected[i].transform.position, piece.transform.position);
            float speed = Mathf.SmoothStep(maxSpeed, minSpeed, (distance / pickupRadius)* (distance / pickupRadius))*Time.deltaTime;

            if (distance < 0.5f)
            {
                Destroy(pickupsDetected[i].gameObject);
                manager.TryEnablePiece(piece);
                continue;
            }
            pickupsDetected[i].GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(pickupsDetected[i].transform.position, piece.transform.position, speed));
            
        }
        for (int i = 0; i < piecesReserved.Count; i++)
        {
            piecesReserved[i].reserved = false;
        }

    }

    public void EjectArmourPickups()
    {
        List<TankArmourPickup> pickups = manager.RemoveAll();

        for (int i = 0; i < pickups.Count; i++)
        {
            Vector2 direction = (pickups[i].transform.position - transform.position).normalized;
            pickups[i].GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
        }
        
    }


    private TankArmourPickup CreateArmourPickupFromPiece(TankArmourPiece piece)
    {
        return Instantiate(pickupPrefab, piece.transform.position, Quaternion.identity);
    }

    private List<TankArmourPickup> DetectArmourPickups()
    {
        List<TankArmourPickup> pickupsDetected = new List<TankArmourPickup>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            TankArmourPickup pickup = colliders[i].GetComponent<TankArmourPickup>();
            if (pickup)
            {
                
                pickupsDetected.Add(pickup);
            }
        }
        return pickupsDetected;
    }
	
}
