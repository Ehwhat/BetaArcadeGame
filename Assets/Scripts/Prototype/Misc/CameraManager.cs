using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {

    public Camera camera;
    public Bounds initalBounds;
    public Vector2 offset;
    public float moveSpeed = 10;
    public float zoomSpeed = 10;
    public GameManager gameManager;

    private Bounds bounds;
    private float widthRatio;

    private void Start()
    {
        bounds = initalBounds;
        widthRatio = initalBounds.size.x / initalBounds.size.y;
    }

    private void Update()
    {
        Bounds newBounds = new Bounds(gameManager.players[0].transform.position, Vector3.zero);

        for (int i = 1; i < 4; i++)
        {
            if (!gameManager.IsPlayerValid(i))
                continue;
            PlayerTankManager tank = gameManager.players[i];
            newBounds.Encapsulate(tank.transform.position);
        }

        newBounds.size += (Vector3)offset;

        bounds = new Bounds(Vector2.MoveTowards(bounds.center, newBounds.center, moveSpeed * Time.deltaTime), Vector2.MoveTowards(bounds.size, newBounds.size, zoomSpeed * Time.deltaTime));

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            camera.orthographicSize = bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            camera.orthographicSize = bounds.size.y / 2 * differenceInSize;
        }

        transform.position = new Vector3(bounds.center.x, bounds.center.y, -20f);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(bounds.center, bounds.size-(Vector3)offset);
    }

}
