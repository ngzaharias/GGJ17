using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    public Transform TOFollow;

    public Vector3 FollowOffset = new Vector3(0,30,5);

	// Use this for initialization
	void Start () 
    {
	
	}



    Vector3 Vel;
    Vector3 NewPos;

	// Update is called once per frame
	void LateUpdate () 
    {

        NewPos = Vector3.SmoothDamp(NewPos,
            TOFollow.position ,
            ref Vel,
            0.1f, 100);


        transform.position = NewPos + FollowOffset;

        transform.LookAt(NewPos);
	
	}
}
