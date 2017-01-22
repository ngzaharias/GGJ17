using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	void FixedUpdate ()
	{
		transform.forward = transform.position - Camera.main.transform.position;
	}
}
