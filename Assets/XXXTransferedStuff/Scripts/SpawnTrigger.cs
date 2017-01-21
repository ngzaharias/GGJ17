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


    public void SpawnStuff()
    {
        GM.SpawnLadyAt(SpawnCount, SpawnPos.position);
    }


    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            SpawnStuff();
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
