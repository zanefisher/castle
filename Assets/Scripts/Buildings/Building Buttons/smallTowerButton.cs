using UnityEngine;
using System.Collections;

public class smallTowerButton : MonoBehaviour {

	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			OpenSmallTowerMenu();
		}
	}


	GameObject smallTowerMenu;
	void Start(){
		smallTowerMenu = GameObject.Find ("SmallTowerMenu");
	}

	bool smallTowerMenuActive;
	void OpenSmallTowerMenu(){
		smallTowerMenuActive = !smallTowerMenuActive;
		smallTowerMenu.SetActive (smallTowerMenuActive);
	}

	public void UpgradeToTower(){

	}
}
