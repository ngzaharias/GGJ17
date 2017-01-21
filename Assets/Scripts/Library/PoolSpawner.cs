using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSpawner : MonoBehaviour
{
	public int m_Size;
	public GameObject m_Prefab;
	private Queue<GameObject> m_Objects;

	public void Initalise(int Size, GameObject Prefab)
	{
		m_Size = Size;
		m_Prefab = Prefab;
		GeneratePool(m_Size, m_Prefab);
	}

	private void GeneratePool(int size, GameObject prefab)
	{
		m_Objects = new Queue<GameObject>();
		for (int i = 0; i < size; ++i)
		{
			GameObject poolObject = CreateObject(prefab);
			m_Objects.Enqueue(poolObject);
		}
	}

	private GameObject CreateObject(GameObject prefab)
	{
		GameObject poolObject = Instantiate(prefab);
		poolObject.transform.SetParent(transform);
		poolObject.SetActive(false);
		return poolObject;
	}

	public void Push(GameObject poolObject)
	{
		poolObject.transform.SetParent(transform);
		poolObject.SetActive(false);
		m_Objects.Enqueue(poolObject);
	}

	public GameObject Pop()
	{
		GameObject poolObject = m_Objects.Dequeue();
		if (poolObject == null)
		{
			poolObject = CreateObject(m_Prefab);
		}

		return poolObject;
	}
}
