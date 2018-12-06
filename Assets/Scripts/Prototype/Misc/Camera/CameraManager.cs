using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public enum CameraMode
    {
        TrackingPlayers,
        Panning,
        Intro
    }

    public CameraMode cameraMode = CameraMode.TrackingPlayers;

    public Camera camera;
    public Bounds initalBounds;
    public Vector2 offset;
    public float moveSpeed = 10;
    public float zoomSpeed = 10;
    public GameManager gameManager;

    private float widthRatio;
    private bool moveToNextStage = false;
    private bool isIntroRunning = false;
    private Bounds lastBounds;

    private void Start()
    {
        lastBounds = initalBounds;
        widthRatio = initalBounds.size.x / initalBounds.size.y;
        RunIntro();
    }

    private void Update()
    {
        switch (cameraMode)
        {
            case CameraMode.TrackingPlayers:
                TrackPlayers();
                break;
            case CameraMode.Panning:
                break;
            case CameraMode.Intro:
                if(Input.GetKeyDown("o")){
                    MoveToNextStage();
                }
                break;
            default:
                break;
        }
        
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(lastBounds.center, lastBounds.size-(Vector3)offset);
    }


    private void TrackPlayers()
    {
        Bounds bounds = LerpBounds(lastBounds,GetCameraTrackingBounds());

        camera.orthographicSize = DetermineOrthographicSize(bounds);

        transform.localPosition = new Vector3(bounds.center.x, bounds.center.y, -20f);
        lastBounds = bounds;
    }

    private Bounds LerpBounds(Bounds bounds, Bounds newBounds)
    {
        return new Bounds(Vector2.MoveTowards(bounds.center, newBounds.center, moveSpeed * Time.deltaTime), Vector2.MoveTowards(bounds.size, newBounds.size, zoomSpeed * Time.deltaTime));
    }

    private float DetermineOrthographicSize(Bounds bounds)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            return bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            return bounds.size.y / 2 * differenceInSize;
        }
    }

    private Bounds GetCameraTrackingBounds()
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

        return newBounds;
    }

    public void MoveToNextStage()
    {
        moveToNextStage = true;
    }

    private void RunIntro()
    {
        StartCoroutine(IntroEnumerator());
    }

    private IEnumerator IntroEnumerator()
    {
        StartCoroutine(DelayMoveToNextStage(4));
        while (!moveToNextStage)
        {
            PanAndZoomTowards(Vector3.zero, 22, 20, 10);
            yield return new WaitForEndOfFrame();
        }
        moveToNextStage = false;
        for (int i = 0; i < 4; i++)
        {
            if (gameManager.IsPlayerValid(i))
            {
                StartCoroutine(DelayMoveToNextStage(2));
                PlayerTankManager player = gameManager.players[i];
                while (!moveToNextStage)
                {
                    PanAndZoomTowards(player.transform.position, 10, 40, 10);
                    yield return new WaitForEndOfFrame();
                }
                moveToNextStage = false;
            }
        }
        StartCoroutine(DelayMoveToNextStage(4));
        Bounds bounds = GetCameraTrackingBounds();
        float zoom = DetermineOrthographicSize(bounds);
        lastBounds = bounds;
        while (!moveToNextStage)
        {
            PanAndZoomTowards(bounds.center, zoom, 20, 10);
            yield return new WaitForEndOfFrame();
        }
        moveToNextStage = false;
        cameraMode = CameraMode.TrackingPlayers;
    }

    private void PanAndZoomTowards(Vector3 target, float zoom, float moveSpeed, float zoomSpeed)
    {
        target = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(target.x, target.y, transform.localPosition.z);
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, zoom, zoomSpeed * Time.deltaTime);
    }

    

    private IEnumerator DelayMoveToNextStage(float time)
    {
        yield return new WaitForSeconds(time);
        MoveToNextStage();
    }

}
