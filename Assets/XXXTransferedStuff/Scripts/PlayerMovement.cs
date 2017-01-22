using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float Speed = 10;
    Vector3 Velocity = Vector3.zero;
    Transform MyTransform;
    Rigidbody MyRigid;

    public float MovmentOffsetAngle = 45;

    // Use this for initialization
    void Start () 
    {
        MyTransform = transform;
        MyRigid = GetComponent<Rigidbody>();
    }

    Vector3 Vup = Vector3.up;
    void Update()
    {
        float DT = Time.deltaTime;


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Velocity.x = x;
        Velocity.z = z;
        Velocity.y = 0;

        Velocity.Normalize();
        Velocity = Quaternion.AngleAxis(MovmentOffsetAngle, Vup) * Velocity;

       // MyTransform.position += Velocity *Speed* DT;

        MyRigid.velocity = Velocity * Speed;//* DT;
        if (Velocity.sqrMagnitude != 0)
        {
            MyTransform.forward = Velocity;
        }

    }
}


// loop game manager
// Chase
// Females
//Dudes(NPC)