using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TankMovement : MonoBehaviour {

    public Vector2 targetVector = Vector2.up;
    public float targetSpeed = 0;

    public float speedModifer = 100;
    public float rotationSpeed = 270;
    public float sideDragFactor = 20;
    public float frontDragFactor = 16;
    public float reverseFactor = 0.6f;
    public float forwardFactor = 1f;

    private Rigidbody2D rigidbody;

    private Vector2 lastVelocityDirection;
    private bool boosting;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        lastVelocityDirection = targetVector;
	}
	
	void Update () {
        if (targetVector.sqrMagnitude > 0)
        {
            float targetAngle = Vector2.SignedAngle(Vector2.up, targetVector);
            rigidbody.rotation = Mathf.MoveTowardsAngle(rigidbody.rotation, targetAngle, (rotationSpeed * Time.deltaTime)/rigidbody.mass);
        }

        float dotModifer = Mathf.SmoothStep(reverseFactor, forwardFactor, (Vector2.Dot(transform.up, targetVector) + 1) * 0.5f);
        rigidbody.AddForce(transform.up * targetSpeed * speedModifer * dotModifer);

        ApplyForwardDrag();
        ApplySideDrag();
    }

    void ApplyForwardDrag()
    {
        Vector2 forwardVelocity = Vector3.Project(rigidbody.velocity, transform.up);
        rigidbody.AddForce((-forwardVelocity * rigidbody.mass) * frontDragFactor);
    }

    void ApplySideDrag()
    {
        Vector2 sideVelocity = Vector3.Project(rigidbody.velocity, transform.right);
        Vector2 forwardVelocity = Vector3.Project(rigidbody.velocity, transform.up);
        if (forwardVelocity.sqrMagnitude > 0.1f)
        {
            rigidbody.AddForce((-sideVelocity*rigidbody.mass) * sideDragFactor);
        }
        else
        {
            rigidbody.AddForce((-sideVelocity * rigidbody.mass) * frontDragFactor);
        }
    }

    public void ApplyOneTimeBoost(float multiplier, float length)
    {
        if (!boosting)
        {
            boosting = true;
            StartCoroutine(BoostRoutine(multiplier, length));
        }
    }

    IEnumerator BoostRoutine(float multiplier, float length)
    {
        float originalSpeed = speedModifer;
        speedModifer *= multiplier;
        yield return new WaitForSeconds(length);
        speedModifer = originalSpeed;
        boosting = false;
    }

}
