using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class productionTab : MonoBehaviour {

	public List<GameObject> buildings;
	public Text tabText;

	void Start(){
		DeselectTab ();
	}

	public void SelectTab(){
		tabText.color = Color.black;
		foreach (GameObject building in buildings) {
			building.SetActive (true);
		}
	}	
	
	public void DeselectTab(){
		tabText.color = Color.gray;
		foreach (GameObject building in buildings) {
			building.SetActive (false);
		}
	}

}
