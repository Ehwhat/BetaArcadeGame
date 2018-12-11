using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpotlight : MonoBehaviour {

    private static bool isFollowing = false;
    private static Transform followTransform;

    public Vector3 pathCenter;
    [Range(0,1)]
    public float phase = 0;
    public float height = 10;
    public float heightSpeed = 0;
    public float width = 10;
    public float widthSpeed = 0;

    void Start () {
		
	}
	
	void Update () {
		if(!isFollowing)
        {
            Vector3 pathPoint = pathCenter + new Vector3((Mathf.Sin((Time.time * widthSpeed) + phase * width) * width), (Mathf.Sin((Time.time * heightSpeed) + phase * height) * height));
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(pathPoint.x, pathPoint.y, transform.position.z), 20*Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(followTransform.position.x, followTransform.position.y, transform.position.z), 20 * Time.deltaTime);
        }
	}

    public static void FollowTransform(Transform transform)
    {
        followTransform = transform;
        isFollowing = true;
    }

    public static void StopFollowing()
    {
        isFollowing = false;
    }
}
