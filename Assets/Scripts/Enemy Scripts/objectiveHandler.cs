using UnityEngine;
using System.Collections;

public class objectiveHandler : MonoBehaviour {

	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			Camera.main.GetComponent<mainCameraScript>().SnapToPosition (gameObject);
		}
	}
}
