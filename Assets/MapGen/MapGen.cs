using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets;
using System;
using Delaunay;
using Delaunay.Geo;


public class MapGen : MonoBehaviour {
	static Mesh CreateMesh(Province prov)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = prov.Border.Select(v => new Vector3(v.x, v.y, 0)).ToArray();
		m.uv = prov.Border.ToArray();
		var triangles = new List<int>();
		Debug.Log(prov.Name);
		Debug.Log("vertices: " + prov.Border.Count);
		for (int i = 1; i < prov.Border.Count - 1; i++)
		{
			triangles.Add(0);
			triangles.Add(i);
			triangles.Add(i + 1);
		}
		Debug.Log("triangles: " + triangles.Aggregate("", (s, i) => s + " " + i));
		m.triangles = triangles.ToArray();
		m.RecalculateNormals();
		
		return m;
	}
	
	static Mesh CreateBorder(Mesh m, float w)
	{
		Mesh b = new Mesh();
		b.name = "Border";
		var vertices = m.vertices.Select(v => new Vector3(v.x, v.y, v.z - 0.01f)).ToList();
		for (int i = 0; i < m.vertexCount; i++)
		{
			int prev = (i + m.vertexCount - 1) % m.vertexCount;
			int next = (i + 1) % m.vertexCount;
			var vec1 = (m.vertices[prev] - m.vertices[i]).normalized;
			var vec2 = (m.vertices[next] - m.vertices[i]).normalized;
			Vector3 delta;
			if (Vector3.Cross(vec1, vec2).z < 0)
			{
				delta = -(vec1 + vec2);
			}
			else
				delta = vec1 + vec2;
			var newVertex = m.vertices[i] + delta * w + new Vector3(0, 0, -0.01f);
			vertices.Add(newVertex);
		}
		var triangles = new List<int>();
		for (int i = 0; i < m.vertexCount; i++)
		{
			int next = (i + 1) % m.vertexCount;
			int top = i + m.vertexCount;
			int topNext = next + m.vertexCount;
			triangles.Add(top);
			triangles.Add(i);
			triangles.Add(next);
			triangles.Add(next);
			triangles.Add(topNext);
			triangles.Add(top);
		}
		b.vertices = vertices.ToArray();
		b.triangles = triangles.ToArray();
		b.uv = b.vertices.Select(v => new Vector2(v.x, v.y)).ToArray();
		b.RecalculateNormals();
		return b;
	}
	
	// Use this for initialization
	void Start () {
		var map = GameMap.Generate();
		foreach (var p in map.GetProvinces())
		{
			var region = new GameObject("NewRegion");
			var meshFilter = region.AddComponent<MeshFilter>();
			meshFilter.mesh = CreateMesh(p);
			var renderer = region.AddComponent<MeshRenderer>();
			renderer.material.shader = Shader.Find("Toon/Basic");
			renderer.material.SetColor("_Color", Color.red);
			region.transform.parent = gameObject.transform;
			
			var borderObject = new GameObject("NewBorder");
			var bMeshFilter = borderObject.AddComponent<MeshFilter>();
			bMeshFilter.mesh = CreateBorder(meshFilter.mesh, 0.2f);
			var bRenderer = borderObject.AddComponent<MeshRenderer>();
			bRenderer.material.shader = Shader.Find("Toon/Basic");
			bRenderer.material.SetColor("_Color", Color.black);
			borderObject.transform.parent = region.transform;
			TestScript regionScript = region.AddComponent<TestScript>();
			regionScript.border = borderObject;
			region.AddComponent<MeshCollider>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public interface IGameMap
{
	IEnumerable<Province> GetProvinces();
	
	bool Bordered(int province1, int province2);
}

public class GameMap : IGameMap
{
	private List<Province> provinces;
	private Dictionary<Tuple<int, int>, bool> matrix;
	public IEnumerable<Province> GetProvinces()
	{
		return provinces;
	}
	
	public GameMap()
	{
		provinces = new List<Province>();
		matrix = new Dictionary<Tuple<int, int>, bool>();
	}
	
	public bool Bordered(int province1, int province2)
	{
		return matrix.ContainsKey(new Tuple<int, int>(province1, province2));
	}
	
	public void AddProvince(Province province)
	{
		provinces.Add(province);
	}
	public void AddBorder(int province1, int province2)
	{
		matrix[new Tuple<int, int>(province1, province2)] = true;
		matrix[new Tuple<int, int>(province2, province1)] = true;
	}

	public static GameMap Generate(){
		GameMap gameRes = new GameMap();
		int n = 50;
		var points = new List<Vector2>();
		var colors = new List<uint> ();
		var lines = new List<Tuple<Vector2, Vector2>>();
		for (int i=0; i<n; i++) {
			points.Add(new Vector2(UnityEngine.Random.Range(-10.0F, 10.0F),UnityEngine.Random.Range(-10.0F, 10.0F)));
			colors.Add (0);
		}
		Delaunay.Voronoi v = new Delaunay.Voronoi (points, colors, new Rect (-10.0F, -10.0F, 20.0F, 20.0F));
		var regions = v.SiteCoords ();
		foreach (var pts in v.Regions()) {
			var prov = new Province();
			prov.Border = pts.ToList();
			prov.Name = "name"; // TODO: generate cool name
			gameRes.AddProvince(prov);
		}
		return gameRes;
	}
}

public class Province
{
	public string Name { get; set; }
	
	public List<Vector2> Border { get; set; }
}