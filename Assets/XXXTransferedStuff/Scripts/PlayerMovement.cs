using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float Speed = 10;
    Vector3 Velocity = Vector3.zero;
    Transform MyTransform;
    Rigidbody MyRigid;

    public float MovmentOffsetAngle = 45;


    int LadyNear = 0;

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
        float Slowed = (Speed * ( 1 - (0.06f *LadyNear)));
        if (Slowed < 0.3f)
        {
            Slowed = 0.3f;
        }

        MyRigid.velocity = Velocity * Slowed;//* DT;
        if (Velocity.sqrMagnitude != 0)
        {
            MyTransform.forward = Velocity;
        }

    }


    void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<LynxLady>() != null)
        {
            LadyNear++;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<LynxLady>() != null)
        {
            LadyNear--;
        }
    }
}


// loop game manager
// Chase
// Females
//Dudes(NPC)