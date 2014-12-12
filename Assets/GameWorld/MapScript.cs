using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;
using GameWorld;
using GameWorld.Players;
using GameWorld.Map;

public class MapScript : MonoBehaviour {
	
	GameMap map;
	static Mesh CreateMesh(Province prov)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = prov.Border.Select(v => new Vector3(v.x, v.y, 0)).ToArray();
		m.uv = prov.Border.ToArray();
		var triangles = new List<int>();
		for (int i = 1; i < prov.Border.Count - 1; i++)
		{
			triangles.Add(0);
			triangles.Add(i);
			triangles.Add(i + 1);
		}
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
		//move it to some MapScript MonoBehaviour, attach MapScript to child GameObject of Main GO and add link in Main:
		//public MapScript Map = blah blah blah;
		map = GameMap.Generate();
		foreach (var p in map.provinces)
		{
			var provinceGO = new GameObject(p.Name + " Province");
			var meshFilter = provinceGO.AddComponent<MeshFilter>();
			meshFilter.mesh = CreateMesh(p);
			var renderer = provinceGO.AddComponent<MeshRenderer>();
			renderer.material.shader = Shader.Find("Toon/Basic");
			renderer.material.SetColor("_Color", Color.red);
			provinceGO.transform.parent = gameObject.transform;
			
			var borderObject = new GameObject("NewBorder");
			var bMeshFilter = borderObject.AddComponent<MeshFilter>();
			bMeshFilter.mesh = CreateBorder(meshFilter.mesh, 0.2f);
			var bRenderer = borderObject.AddComponent<MeshRenderer>();
			bRenderer.material.shader = Shader.Find("Toon/Basic");
			bRenderer.material.SetColor("_Color", Color.black);
			borderObject.transform.parent = provinceGO.transform;
			provinceGO.AddComponent<MeshCollider>();
			ProvinceScript provinceScript = provinceGO.AddComponent<ProvinceScript>();
			provinceScript.border = borderObject;
			provinceScript.name = p.Name;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
