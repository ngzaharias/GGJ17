using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMesh : MonoBehaviour 
{

    SkinnedMeshRenderer TheMeshRend;

    public List<Mesh> MeshOptions;

	// Use this for initialization
	void Start ()
    {
        float RandI = Random.Range(0f, MeshOptions.Count);
        if (RandI != MeshOptions.Count)
        {
            TheMeshRend.sharedMesh = MeshOptions[(int)RandI];
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
