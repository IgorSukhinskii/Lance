using UnityEngine;
using System.Collections;

public class ClickableProvince : MonoBehaviour {
	
	public GameObject border;
	private bool selected = false;
	void OnMouseDown()
	{
		Debug.Log ("mouse down");
		if (selected)
			border.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		else
			border.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
		selected = !selected;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}