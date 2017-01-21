using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class test_ConstantForceController : MonoBehaviour
{
	[Range(-100.0f, 100.0f)]
	public float m_ForceMultiplier = 1.0f;
	public Vector3 m_ForceDirection = Vector3.up;

	private ConstantForce[] cached_ConstantForces;

	private void Awake()
	{
		cached_ConstantForces = GetComponentsInChildren<ConstantForce>();
	}

	private void Update()
	{
		Vector3 force = m_ForceDirection * m_ForceMultiplier;
		SetForce(force);
	}

	private void SetForce(Vector3 force)
	{
		foreach (ConstantForce constant in cached_ConstantForces)
		{
			constant.force = force;
		}
	}
}
