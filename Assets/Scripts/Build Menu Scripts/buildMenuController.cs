using UnityEngine;
using System.Collections;

public class buildMenuController : MonoBehaviour {

	public GameObject buildMenu;

	void Start(){
		//Why is this giving me an error when it's working?
		buildMenu.SetActive (false);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.B)) {
			OpenBuildMenu();
		}
	}

	bool buildMenuStatus = true;
	public void OpenBuildMenu(){
		buildMenu.SetActive (buildMenuStatus);
		buildMenuStatus = !buildMenuStatus;
	}
}
