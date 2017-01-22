using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour 
{
    public List<Transform> ThePath;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
    }
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        for(int i = 0; i < ThePath.Count-1; ++i)
        {
            Gizmos.DrawLine(ThePath[i].position, ThePath[i + 1].position);
        }
    }
}
