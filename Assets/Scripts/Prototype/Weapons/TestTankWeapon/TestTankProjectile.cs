using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTankProjectile : IProjectile
{

    public Vector3 position;
    public Vector3 direction;

    public TestTankWeapon.TestTankWeaponData data;
    GameObject representation;
    System.Action<TestTankProjectile> finished;

    public void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null)
    {
        position = firedPosition;
        direction = firedDirection;
        data = (TestTankWeapon.TestTankWeaponData)firedData;

        representation = GameObject.Instantiate(data.representationPrefab, position, Quaternion.identity);
        finished = data.finishedCallback;
    }

    public void Update(float deltaTime)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 10 * deltaTime);
        if (hit)
        {
            position = hit.point;
            representation.transform.position = position;
            finished(this);
            GameObject.Destroy(representation, 2);
        }
        else
        {
            position += direction * 10 * deltaTime;
            representation.transform.position = position;
        }
        
    }
}
