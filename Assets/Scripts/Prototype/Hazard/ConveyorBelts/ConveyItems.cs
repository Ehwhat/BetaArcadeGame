using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyItems : MonoBehaviour {

    public LayerMask tankLayermask;
    public Transform prefabHolder;
    public GameObject[] conveyablePrefabs;
    public float length = 10;
    public float width = 3;
    public float spaceBetweenItemsMin = 4;
    public float spaceBetweenItemsMax = 8;
    public float speed = -20;

    private List<GameObject> currentConveyedObjects = new List<GameObject>();

	void Start () {
        PreWarm();

    }
	
	// Update is called once per frame
	void Update () {

        float endY = (transform.position.y + ((transform.up * length / 2) * Mathf.Sign(speed)).y);
        float beginY = (transform.position.y - ((transform.up * length / 2) * Mathf.Sign(speed)).y);
        for (int i = 0; i < currentConveyedObjects.Count; i++)
        {
            currentConveyedObjects[i].transform.position += transform.up * speed * Time.deltaTime;
            if(currentConveyedObjects[i].transform.localPosition.y > length/2)
            {
                currentConveyedObjects[i].transform.position = new Vector3(currentConveyedObjects[i].transform.position.x, beginY, currentConveyedObjects[i].transform.position.z);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(width, length), 0, tankLayermask);
        List<TankManager> tanks = new List<TankManager>();
        for (int i = 0; i < colliders.Length; i++)
        {
            TankManager tank = colliders[i].attachedRigidbody.GetComponent<TankManager>();
            if (tank && !tanks.Contains(tank))
            {
                tanks.Add(tank);
            }
        }
        for (int i = 0; i < tanks.Count; i++)
        {
            tanks[i].transform.position += transform.up * speed * Time.deltaTime;
        }

	}

    private void PreWarm()
    {
        float distance = 0;
        while (distance < length)
        {
            Vector3 position = transform.position + (transform.up * (distance - (length/2)));
            currentConveyedObjects.Add(CreatePrefab(position));
            distance += Random.Range(spaceBetweenItemsMin, spaceBetweenItemsMax);
        }
    }

    private GameObject CreatePrefab(Vector3 position)
    {
        GameObject prefab = conveyablePrefabs[Random.Range(0, conveyablePrefabs.Length - 1)];
        prefab = Instantiate(prefab, position, Quaternion.Euler(0, 0, Random.value * 360), prefabHolder);
        return prefab;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + transform.up * (-length / 2), transform.position + transform.up * (length / 2));
    }

}
