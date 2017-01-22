using UnityEngine;
using System.Collections;

public class LynxLady : MonoBehaviour
{
    public float Speed = 10;

    float SpeedVatiant = 0.2f;

    public GameObject Blurz;

    public GameManager GM;

    Transform PLayerTransform;
    Vector3 Velocity = Vector3.zero;

    Rigidbody MyRigid;

    Transform MyTransform;

    public bool SmellDude = false;
    public Transform DudeTrans;
    Animator MYANim;
    float LifeTime = 0;
	// Use this for initialization
	void Start () 
    {

        MYANim = GetComponent<Animator>();
        MyTransform = transform;

        MyTransform.position = new Vector3(MyTransform.position.x
            , GM.PeopleYlevel, MyTransform.position.z);

        SpeedVatiant = Random.Range(-SpeedVatiant, SpeedVatiant);

        PLayerTransform = GM.PlayerTransform;
        MyRigid = GetComponent<Rigidbody>();
	}
    bool DiveonDude = false;
	// Update is called once per frame
	void Update () 
    {
        float DT = Time.deltaTime;
        LifeTime += DT;
        if (SmellDude)
        {
            if (!DiveonDude)
            {
                Blurz.SetActive(true);
                DiveonDude = true;
                MYANim.SetBool("IsDead", true);
                MYANim.SetTrigger("Dive");
                Destroy(gameObject, 20);
            }
            if (DudeTrans != null)
            {
                Velocity = DudeTrans.position - MyTransform.position;

                Velocity.y = 0;

                Velocity.Normalize();

                MyRigid.velocity = Velocity * (Speed * (1 + (SpeedVatiant*((int)(LifeTime/15))))* (1 + SpeedVatiant));//* DT;

                transform.forward = Velocity;
            }
            else
            {
                MyRigid.velocity = Vector3.zero;
            }
            
        }
        else
        {

            Velocity = PLayerTransform.position - MyTransform.position;

            Velocity.y = 0;

            Velocity.Normalize();

            MyRigid.velocity = Velocity * (Speed * (1 + (SpeedVatiant*((int)(LifeTime/15))))* (1 + SpeedVatiant));//* DT;
            transform.forward = Velocity;
            //MyTransform.position += Velocity *Speed* DT;
        }
	
	}

    bool Hasded = false;
    void OnBecameInvisible() 
    {
        if (SmellDude && !Hasded)
        {
            Hasded = true;
            Destroy(gameObject, 0.2f);
        }
    }



}
