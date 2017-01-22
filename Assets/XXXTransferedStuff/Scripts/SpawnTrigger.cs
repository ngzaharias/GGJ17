using UnityEngine;
using System.Collections;

public class SpawnTrigger : MonoBehaviour 
{
    public GameObject ToSpawn;

    public int SpawnCount = 10;


    public Transform SpawnPos; 

    [HideInInspector]
    public GameManager GM;

    void Start ()
    {
        GM = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    int ToSpawnCount = 0;
    float SpawnInteval = 0.1f;
    float Timer = -0.7f;

    void Update()
    {
        if (ToSpawnCount > 0)
        {
            Timer += Time.deltaTime;
            if (Timer > SpawnInteval)
            {
                Timer = 0;
                GM.SpawnLadyAt(1, SpawnPos.position);
                ToSpawnCount--;
            }
        }
    }

    public void SpawnStuff()
    {
        
    }


    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            ToSpawnCount += SpawnCount;
        }
    }

    void OnTriggerExit(Collider other) 
    {
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(SpawnPos.position,Vector3.one);
    }
}
