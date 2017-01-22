using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public float Score = 0;
    public Text ScoreText;


    [HideInInspector]
    public float PeopleYlevel = 0.8f;

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

        PlayerTransform.position = new Vector3(PlayerTransform.position.x
            , PeopleYlevel, PlayerTransform.position.z);

        for (int i = 0; i < 30; ++i)
        {
            float Angle = Random.Range(0, 360);

            TempV = new Vector3(0, 0, SpawnDist);

            TempV = Quaternion.AngleAxis(Angle, Vup) * TempV;

            TempV = PlayerTransform.position + TempV; 
            Score++;
            SpawnedGO =  Instantiate(LadyPrefab, TempV, Quaternion.identity) as GameObject;
            SpawnedGO.GetComponent<LynxLady>().GM = this;
        }


	}

   // float SpawnRate = 0.1f;

    public void SpawnLadyAt(int Count,Vector3 SpawnPos)
    {
        for (int i = 0; i < Count; ++i)
        {
            Score++;
            SpawnedGO =  Instantiate(LadyPrefab, SpawnPos, Quaternion.identity) as GameObject;
            SpawnedGO.GetComponent<LynxLady>().GM = this;
        }
    }
	

	// Update is called once per frame
	void Update () 
    {
        Score += Time.deltaTime;
        ScoreText.text = ((int)Score).ToString();
        
        MAP.SetCoord(PlayerTransform.position);

        Timer += Time.deltaTime;

        if (Timer > SpawnTime)
        {
            Timer = 0;

            for (int i = 0; i < SpawnCound; ++i)
            {
                float Angle = Random.Range(0, 360);

                TempV = new Vector3(0, 0, SpawnDist);

                TempV = Quaternion.AngleAxis(Angle, Vup) * TempV;

                TempV = PlayerTransform.position + TempV; 
                Score++;
                SpawnedGO =  Instantiate(LadyPrefab, TempV, Quaternion.identity) as GameObject;
                SpawnedGO.GetComponent<LynxLady>().GM = this;
            }
        }

	
	}
}
