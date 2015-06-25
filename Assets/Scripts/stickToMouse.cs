using UnityEngine;
using System.Collections;

public class stickToMouse : MonoBehaviour {

	Vector3 mousePos;
	Vector3 objPos;

	void Start(){
		gameObject.layer = 2;
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			mousePos = hitDist.point;
		}
		objPos = new Vector3 (mousePos.x, transform.position.y, mousePos.z);
		transform.position = objPos;

		if(Input.GetMouseButtonDown(0) && hitDist.collider.gameObject.name == "Ground"){
			gameObject.layer = 0;
			Destroy(this);
		}

		if (Input.GetMouseButtonDown (1)) {
			Destroy (gameObject);
		}
	}
}
