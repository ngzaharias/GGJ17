using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

    public Transform PlayerTransform;
    public GameObject PlayerGO;
    public PlayerMovement PlayerScript;

    public GameObject LadyPrefab;

    public float SpawnTime = 5;
    float Timer = 0;
    public float SpawnCound = 3;

    test_ProceduralWorld MAP;

    Vector3 TempV;
    Vector3 Vup = Vector3.up;
    float SpawnDist = 100;
    GameObject SpawnedGO;

	// Use this for initialization
	void Start () 
    {
        MAP = GetComponent<test_ProceduralWorld>();
	}

    public void SpawnLadyAt(int Count,Vector3 SpawnPos)
    {
        for (int i = 0; i < Count; ++i)
        {
            SpawnedGO =  Instantiate(LadyPrefab, SpawnPos, Quaternion.identity) as GameObject;
            SpawnedGO.GetComponent<LynxLady>().GM = this;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		MAP.SetCoord(PlayerTransform.position);

        Timer += Time.deltaTime;

        if (Timer > SpawnTime)
        {
            Timer = 0;

            for (int i = 0; i < SpawnCound; ++i)
            {
                float Angle = Random.Range(0, 360);

                TempV = new Vector3(0, 2, SpawnDist);

                TempV = Quaternion.AngleAxis(Angle, Vup) * TempV;

                TempV = PlayerTransform.position + TempV; 

                SpawnedGO =  Instantiate(LadyPrefab, TempV, Quaternion.identity) as GameObject;
                SpawnedGO.GetComponent<LynxLady>().GM = this;
            }
        }

	
	}
}
