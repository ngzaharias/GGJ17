using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class test_ApplyForce : MonoBehaviour
{
	public int m_MouseButton;

	public float m_ForceMultiplier;
	public Vector3 m_ForceDirection;

	public float m_TorqueMultiplier;
	public Vector3 m_TorqueAxis;

	public Transform cached_Root;
	private Rigidbody cached_Rigidbody;

	private void Awake()
	{
		cached_Rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (Input.GetMouseButton(m_MouseButton) == true)
		{
			cached_Rigidbody.AddForce((-cached_Root.right + cached_Root.up) * m_ForceMultiplier);
			//cached_Rigidbody.AddRelativeForce(m_ForceDirection * m_ForceMultiplier);
			cached_Rigidbody.AddRelativeTorque(m_TorqueAxis * m_TorqueMultiplier);
		}
	}
}
