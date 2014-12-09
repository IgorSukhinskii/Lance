using UnityEngine;
using System.Collections;
using System;
public class ProvinceScript : MonoBehaviour {
	private static ProvinceScript _selectedProvince;
	public static event EventHandler SelectedProvinceChanged;
	public static ProvinceScript SelectedProvince {
		get {
			return _selectedProvince;
		}
		set {
			if (_selectedProvince != null) 
				_selectedProvince.border.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);

			_selectedProvince = value;
			if (_selectedProvince != null) 
				_selectedProvince.border.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
			
			if (SelectedProvinceChanged != null)
				SelectedProvinceChanged (null, null);
		} 
	}
	public string name;
	public GameObject border;
	private bool selected = false;
	void OnMouseDown()
	{
		SelectedProvince = this;			
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}