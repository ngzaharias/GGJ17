using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float Speed = 10;
    Vector3 Velocity = Vector3.zero;
    Transform MyTransform;
    Rigidbody MyRigid;

    public float MovmentOffsetAngle = 45;

    public GameObject SprayEffect;
    public Transform SpratSpawnPos;

    Animator MYANim;

    int LadyNear = 0;

    // Use this for initialization
    void Start () 
    {
        MYANim = GetComponent<Animator>();
        MyTransform = transform;
        MyRigid = GetComponent<Rigidbody>();
    }

    Vector3 Vup = Vector3.up;
    float DeathCheckTImer = 0;
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
            DeathCheckTImer += DT;
            if (DeathCheckTImer > 0.3f)
            {
                MYANim.SetTrigger("Tackled");
            }
        }

        MyRigid.velocity = Velocity * Slowed;//* DT;
        if (Velocity.sqrMagnitude != 0)
        {
            MYANim.SetBool("Running", true);
            MyTransform.forward = Velocity;
        }
        else
        {
            MYANim.SetBool("Running", false);
        }

    }


    void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<LynxLady>() != null)
        {
            other.GetComponent<Animator>().SetTrigger("Dive");
            LadyNear++;
        }
        NPCdude DUDE = other.gameObject.GetComponent<NPCdude>();
        if (DUDE != null)
        {
            DUDE.IsSprayed = true;
            other.gameObject.transform.localScale = DUDE.SprayedScale;
            GameObject GO =  Instantiate(SprayEffect, SpratSpawnPos.position,
                Quaternion.LookRotation(DUDE.transform.position - MyTransform.position)) as GameObject;
            GO.transform.parent = SpratSpawnPos;
            Destroy(GO, 0.4f);
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