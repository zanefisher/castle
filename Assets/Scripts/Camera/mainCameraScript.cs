using UnityEngine;
using System.Collections;

public class mainCameraScript : MonoBehaviour {

	public float panSpeed;

	void Update () {
		HandleEdgePanning ();
		HandleZoom ();
	}

	void HandleEdgePanning (){
		//if (Input.mousePosition.x > Screen.width * 0.9f) {
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (panSpeed * Time.deltaTime, 0f, 0f);
			//}else if(Input.mousePosition.x < Screen.width * 0.1f) {
		}else if(Input.GetKey (KeyCode.A)){
			transform.Translate (-panSpeed * Time.deltaTime, 0f, 0f);
		//}else if(Input.mousePosition.y > Screen.height * 0.9f) {
		}else if(Input.GetKey (KeyCode.W)){
			transform.Translate (0f, panSpeed * Time.deltaTime, 0f);
		//}else if(Input.mousePosition.y < Screen.height * 0.1f) {
		}else if(Input.GetKey (KeyCode.S)){
			transform.Translate (0f, -panSpeed * Time.deltaTime, 0f);
		}
	}

	void HandleZoom(){
		float mouseWheelMovement = Input.GetAxis ("Mouse ScrollWheel");
		if (Camera.main.orthographic) {
			Camera.main.orthographicSize += mouseWheelMovement;
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 2f, 7.5f);
		} else {
			transform.Translate (0f, 0f, -mouseWheelMovement);
			transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y, 5, 20), transform.position.z);
		}
	}
}
