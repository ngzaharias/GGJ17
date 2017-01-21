using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapCoord
{
	public int x;
	public int y;

	public MapCoord(int x, int y)
	{
		this.x = x;
		this.y = y;
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
		return (x << 16) ^ y;
	}

	static public MapCoord Min(params MapCoord[] vals)
	{
		MapCoord min = vals[0];
		for (int i = 1; i < vals.Length; ++i)
		{
			if (vals[i].x < min.x) min.x = vals[i].x;
			if (vals[i].y < min.y) min.y = vals[i].y;
		}
		return min;
	}

	static public MapCoord Max(params MapCoord[] vals)
	{
		MapCoord max = vals[0];
		for (int i = 1; i < vals.Length; ++i)
		{
			if (vals[i].x > max.x) max.x = vals[i].x;
			if (vals[i].y > max.y) max.y = vals[i].y;
		}
		return max;
	}

	static public MapCoord operator +(MapCoord lhs, MapCoord rhs)
	{
		return new MapCoord(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	static public bool operator ==(MapCoord lhs, MapCoord rhs)
	{
		return lhs.x == rhs.x
			&& lhs.y == rhs.y;
	}

	static public bool operator !=(MapCoord lhs, MapCoord rhs)
	{
		return lhs.x != rhs.x
			&& lhs.y != rhs.y;
	}
}

public class test_ProceduralWorld : MonoBehaviour
{
	public MapCoord m_ActiveSize = new MapCoord(3,3);
	public Vector2 m_ObjectOffset;

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
		for (; index.x < max.x; ++index.x)
		{
			index.y = min.y;
			for (; index.y < max.y; ++index.y)
			{
				if (index.x >= coord.x && index.x < coord.x + m_ActiveSize.x
				&& index.y >= coord.y && index.y < coord.y + m_ActiveSize.y)
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

	private void InitialisePool()
	{
		m_PoolObjects = new Queue<GameObject>();

		int size = m_ActiveSize.x * m_ActiveSize.y + m_PoolExtra;
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
		for (int x = 0; x < m_ActiveSize.x; ++x)
		{
			for (int y = 0; y < m_ActiveSize.y; ++y)
			{
				ActivateCoord( new MapCoord(x, y) );
			}
		}
	}

	private void ActivateCoord(MapCoord coord)
	{
		if (IsCoordActive(coord) == true)
			return;

		float x = coord.x * m_ObjectOffset.x;
		float y = coord.y * m_ObjectOffset.y;

		GameObject poolObject = m_PoolObjects.Dequeue();
		poolObject.transform.SetParent(m_Active.transform);
		poolObject.SetActive(true);
		poolObject.transform.position = new Vector3(x, 0, y);

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
