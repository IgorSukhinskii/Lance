using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SquadControlPanelScript : MonoBehaviour {
	[SerializeField]
	public Text title;
	// Use this for initialization
	void Start () {
		ProvinceScript.SelectedProvinceChanged += delegate(object sender, System.EventArgs e) {
			if (ProvinceScript.SelectedProvince != null) {
				this.title.text = "Forces in " + ProvinceScript.SelectedProvince.name;
			}
			else {
				this.title.text = "";
			}
			
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
