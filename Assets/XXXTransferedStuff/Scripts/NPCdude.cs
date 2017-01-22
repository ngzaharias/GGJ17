using UnityEngine;
using System.Collections;

public class NPCdude : MonoBehaviour 
{

    public bool IsSprayed = false;

    public float LifeTime = 5;

    public GameObject DeathPrefap;

    public Vector3 SprayedScale = new Vector3(10, 10, 10);

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
        if (IsSprayed)
        {
            LifeTime -= Time.deltaTime;

            if(LifeTime < 0)
            {
              //  Instantiate(DeathPrefap, transform.position, transform.rotation);
                Destroy(GetComponent<Collider>());
            }
        }
	}


    void OnTriggerEnter(Collider other) 
    {
        if (!IsSprayed)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                IsSprayed = true;
                transform.localScale = SprayedScale;
            }
        }
        else
        { 
            LynxLady LL = other.gameObject.GetComponent<LynxLady>();
            if (LL != null)
            {
                LL.SmellDude = true;
                LL.DudeTrans = transform;
                ((SphereCollider)other).radius *= 0.8f;
            }
        }
    }
}
