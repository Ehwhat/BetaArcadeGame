using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    struct CenterPos
    {
        public Vector2 position;
        public float distance;
    }

    public Camera camera;

    public float moveSpeed = 10;
    public float zoomSpeed = 10;

    public float sizeMax = 20f;
    public float sizeMin = 3f;
    public float sizeOffset = 8f;
    public float sizeMultiplier = 1;
    public float zOffset = -20f;

    public GameManager gameManager;
   
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CenterPos center = CentoridPosition();
        camera.transform.position = Vector2.MoveTowards(transform.position, new Vector3(center.position.x, center.position.y), moveSpeed*Time.deltaTime);
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y,zOffset);
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, Mathf.Clamp((center.distance* sizeMultiplier) + sizeOffset,sizeMin,sizeMax), zoomSpeed*Time.deltaTime);
	}

    CenterPos CentoridPosition()
    {
        CenterPos centerPos = new CenterPos();
        centerPos.position = (Vector2)gameManager.players[0].transform.position;
        float bestDistance = 0;
        for (int i = 1; i < gameManager.players.Length; i++)
        {
            
            if (gameManager.IsPlayerValid(i))
            {
                for (int j = 0; j < gameManager.players.Length; j++)
                {
                    if (i == j || !gameManager.IsPlayerValid(j))
                    {
                        continue;
                    }
                    float distance = Vector2.Distance(gameManager.players[i].transform.position, gameManager.players[j].transform.position);
                    if (distance > bestDistance)
                    {
                        bestDistance = distance;
                    }
                }
                centerPos.position += (Vector2)gameManager.players[i].transform.position;
            }
        }
        centerPos.distance = bestDistance;
        centerPos.position /= 4;
        return centerPos;
    }
}
