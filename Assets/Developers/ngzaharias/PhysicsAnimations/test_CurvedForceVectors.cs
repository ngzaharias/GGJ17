using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class test_CurvedForceVectors : MonoBehaviour
{
	public float m_forceMultiplier = 1.0f;
	public Vector3 m_forceDirection = Vector3.forward;
	public AnimationCurve m_forceCurve = AnimationCurve.Linear(0, 1, 0, 1);

	private Rigidbody cached_Rigidbody;

	void Awake()
	{
		cached_Rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		float evaluate = m_forceCurve.Evaluate(Time.time);
		Vector3 force = m_forceDirection * (evaluate * m_forceMultiplier);
		cached_Rigidbody.AddForce(force, ForceMode.VelocityChange);
	}
}
