using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapCoord
{
	public int column;
	public int row;

	public MapCoord(int column, int row)
	{
		this.column = column;
		this.row = row;
	}

	public bool Equals(MapCoord obj)
	{
		return this == obj;
	}

	public override bool Equals(System.Object obj)
	{
		if (obj is MapCoord)
		{
			return this == (MapCoord)obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (column << 16) ^ row;
	}

	static public MapCoord Min(params MapCoord[] vals)
	{
		MapCoord min = vals[0];
		for (int i = 1; i < vals.Length; ++i)
		{
			if (vals[i].column < min.column) min.column = vals[i].column;
			if (vals[i].row < min.row) min.row = vals[i].row;
		}
		return min;
	}

	static public MapCoord Max(params MapCoord[] vals)
	{
		MapCoord max = vals[0];
		for (int i = 1; i < vals.Length; ++i)
		{
			if (vals[i].column > max.column) max.column = vals[i].column;
			if (vals[i].row > max.row) max.row = vals[i].row;
		}
		return max;
	}

	static public MapCoord operator +(MapCoord lhs, MapCoord rhs)
	{
		return new MapCoord(lhs.column + rhs.column, lhs.row + rhs.row);
	}

	static public bool operator ==(MapCoord lhs, MapCoord rhs)
	{
		return lhs.column == rhs.column
			&& lhs.row == rhs.row;
	}

	static public bool operator !=(MapCoord lhs, MapCoord rhs)
	{
		return lhs.column != rhs.column
			&& lhs.row != rhs.row;
	}
}

public class test_ProceduralWorld : MonoBehaviour
{
	public MapCoord m_ActiveSize = new MapCoord(3,3);
	public Vector3 m_ObjectOffset;

	private GameObject m_Active;
	private GameObject m_Inactive;

	public int m_PoolExtra = 10;
	public GameObject[] m_PoolPrefabs;
	private Queue<GameObject> m_PoolObjects;

	private Dictionary<MapCoord, GameObject> m_TileObjects;

	public MapCoord m_CurrentCoord { get; private set; }

	private void Awake()
	{
		m_Active = new GameObject();
		m_Active.name = "Active";
		m_Active.transform.SetParent(transform);

		m_Inactive = new GameObject();
		m_Inactive.name = "Inactive";
		m_Inactive.transform.SetParent(transform);

		m_TileObjects = new Dictionary<MapCoord, GameObject>();
	}

	private void Start()
	{
		InitialisePool();
		InitialiseTiles();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			SetCoord(new MapCoord(0, 0));
		if (Input.GetKeyDown(KeyCode.F1))
			SetCoord(new MapCoord(1, 1));
		if (Input.GetKeyDown(KeyCode.F2))
			SetCoord(new MapCoord(2, 2));
		if (Input.GetKeyDown(KeyCode.F3))
			SetCoord(new MapCoord(3, 3));
	}

	public bool IsCoordActive(MapCoord coord)
	{
		return m_TileObjects.ContainsKey(coord) == true
			&& m_TileObjects[coord] != null;
	}

	public void SetCoord(MapCoord coord)
	{
		if (coord == m_CurrentCoord)
			return;

		MapCoord min = MapCoord.Min(m_CurrentCoord, coord);
		MapCoord max = MapCoord.Max(m_CurrentCoord + m_ActiveSize, coord + m_ActiveSize);

		MapCoord index = min;
		Queue<MapCoord> activations = new Queue<MapCoord>();
		Queue<MapCoord> deactivations = new Queue<MapCoord>();
		for (; index.column < max.column; ++index.column)
		{
			index.row = min.row;
			for (; index.row < max.row; ++index.row)
			{
				if (index.column >= coord.column && index.column < coord.column + m_ActiveSize.column
				&& index.row >= coord.row && index.row < coord.row + m_ActiveSize.row)
				{
					activations.Enqueue(index);
				}
				else
				{
					deactivations.Enqueue(index);
				}
			}
		}

		foreach (MapCoord deactivate in deactivations)
			DeactivateCoord(deactivate);

		foreach (MapCoord activate in activations)
			ActivateCoord(activate);

		m_CurrentCoord = coord;
	}

	public void SetCoord(Vector3 worldPosition)
	{
		SetCoord( WorldToCoord(worldPosition) );
	}

	public MapCoord WorldToCoord(Vector3 worldPosition)
	{
		Debug.Assert(m_ObjectOffset.x != 0.0f && m_ObjectOffset.z != 0.0f);

		float halfWidth = (m_ActiveSize.column) / 2.0f;
		float halfHeight = (m_ActiveSize.row) / 2.0f;
		float column = Mathf.Ceil(worldPosition.x / m_ObjectOffset.x - halfWidth);
		float row = Mathf.Ceil(worldPosition.z / m_ObjectOffset.z - halfHeight);
		return new MapCoord((int)column, (int)row);
	}

	private void InitialisePool()
	{
		if (m_PoolPrefabs.Length <= 0)
		{
			Debug.LogError("You haven't assigned any prefabs for the pool to use!");
			Debug.Break();
			return;
		}

		m_PoolObjects = new Queue<GameObject>();

		int size = m_ActiveSize.column * m_ActiveSize.row + m_PoolExtra;
		for (int i = 0; i < size; ++i)
		{
			int index = Random.Range(0, m_PoolPrefabs.Length);
			GameObject prefab = m_PoolPrefabs[index];

			GameObject poolObject = Instantiate(prefab);
			poolObject.name = prefab.name;
			poolObject.transform.SetParent(m_Inactive.transform);
			poolObject.SetActive(false);

			m_PoolObjects.Enqueue(poolObject);
		}
	}

	private void InitialiseTiles()
	{
		for (int column = 0; column < m_ActiveSize.column; ++column)
		{
			for (int row = 0; row < m_ActiveSize.row; ++row)
			{
				ActivateCoord( new MapCoord(column, row) );
			}
		}
	}

	private void ActivateCoord(MapCoord coord)
	{
		Debug.AssertFormat(m_PoolObjects.Count > 0, "PoolObjects is Empty!");

		if (IsCoordActive(coord) == true)
			return;

		float x = coord.column * m_ObjectOffset.x;
		float z = coord.row * m_ObjectOffset.z;

		GameObject poolObject = m_PoolObjects.Dequeue();
		poolObject.transform.SetParent(m_Active.transform);
		poolObject.SetActive(true);
		poolObject.transform.position = new Vector3(x, 0, z);

		m_TileObjects[coord] = poolObject;
	}

	private void DeactivateCoord(MapCoord coord)
	{
		if (IsCoordActive(coord) == false)
			return;

		GameObject poolObject = m_TileObjects[coord];
		poolObject.transform.SetParent(m_Inactive.transform);
		poolObject.SetActive(false);

		m_PoolObjects.Enqueue(poolObject);

		m_TileObjects[coord] = null;
	}
}
