using UnityEngine;
using System.Collections;

public class mainCameraScript : MonoBehaviour {

	public float panSpeed;
	public float zoomSpeed;
	bool cannotMoveCamera;

	void Update () {
		if (!cannotMoveCamera) {
			HandleEdgePanning ();
			HandleZoom ();
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log (viewToWorldLeft ().x);
			Debug.Log (viewToWorldRight ().x);
		}
		//HandleSnapping ();
	}

	void HandleEdgePanning (){
		//if (Input.mousePosition.x > Screen.width * 0.9f) {
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Translate (panSpeed * Time.deltaTime, 0f, 0f);
			//}else if(Input.mousePosition.x < Screen.width * 0.1f) {
		}else if(Input.GetKey (KeyCode.LeftArrow)){
			transform.Translate (-panSpeed * Time.deltaTime, 0f, 0f);
		//}else if(Input.mousePosition.y > Screen.height * 0.9f) {
		}else if(Input.GetKey (KeyCode.UpArrow)){
			transform.Translate (0f, panSpeed * Time.deltaTime, 0f);
		//}else if(Input.mousePosition.y < Screen.height * 0.1f) {
		}else if(Input.GetKey (KeyCode.DownArrow)){
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
			//transform.position = new Vector3 (transform.position.x, Mathf.Clamp (transform.position.y, 5, 20), transform.position.z);
		}
	}

	/*public bool finishedSnapping;
	public Vector3 snapPosition;
	void HandleSnapping(){
		if(!finishedSnapping)
	}*/
	Vector3 viewToWorldRight(){
		return Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
	}
	Vector3 viewToWorldLeft(){
		return Camera.main.ViewportToWorldPoint (new Vector3(0, 0.5f, Camera.main.nearClipPlane));
	}

	//TRYING TO USE TRIG. THIS IS THE BEST SO FAR, BUT SUCKS IF THE TARGET IS TOO CLOSE TO THE BASE
	public Transform baseTransform;
	public void SnapToPosition(GameObject target){
		float halfFOV = Camera.main.fieldOfView * 0.5f;
		float angleFromGround = 90f - halfFOV;
		float halfDistance = Vector3.Distance (target.transform.position, baseTransform.position) / 2f;
		Vector3 midPoint = ((target.transform.position - baseTransform.position) / 2f) + baseTransform.position;
		float direction = Vector3.Angle (target.transform.position, baseTransform.position);
		Debug.Log (direction);
		//float height = halfDistance / (Mathf.Tan (halfFOV));
		float height = halfDistance * Mathf.Tan (angleFromGround);
		Vector3 snapPos = new Vector3 (midPoint.x, height * 4f, midPoint.z);
		//transform.position = snapPos;
		StartCoroutine (LerpToPos (snapPos));
	}

	IEnumerator LerpToPos(Vector3 snapPos){
		iTween.MoveTo (gameObject, snapPos, 1f);
		yield return null;
	}


	//JANKY LERPING
	/*public void SnapToPosition(GameObject target){
		Vector3 snapPos = (target.transform.position /2f);
		transform.position = new Vector3 (snapPos.x, transform.position.y, snapPos.z);
		if (target.transform.position.x < viewToWorldLeft ().x) {
			Debug.Log ("left");
			StartCoroutine (ZoomOut(target));
		}
	}

	IEnumerator ZoomOut(GameObject target){
		Debug.Log ("zooming");
		while (target.transform.position.x < viewToWorldLeft().x) {
			cannotMoveCamera = true;
			if(Camera.main.orthographic){
				GetComponent<Camera>().orthographicSize += Time.deltaTime * zoomSpeed;
			}else{
				transform.position += Vector3.up * Time.deltaTime * zoomSpeed;
			}
			yield return null;
		}
		Debug.Log ("done zooming");
		cannotMoveCamera = false;
		yield return null;
	}*/


	/*public void SnapToPosition(GameObject target){
		Vector3 midPoint = ((target.transform.position - baseTransform.position) / 2f) + baseTransform.position;
		Vector3 snapPos = new Vector3 (midPoint.x, transform.position.y + 5f, midPoint.z);
		StartCoroutine (LerpToPos (target, snapPos));
	}

	IEnumerator LerpToPos(GameObject target, Vector3 snapPos){
		iTween.MoveTo (gameObject, iTween.Hash ("position", snapPos, "time", 1f, "oncomplete", "ZoomInOrOut", "oncompleteparams", target));

		yield return null;
	}

	void ZoomInOrOut(GameObject target){
		if (target.transform.position.x < viewToWorldLeft().x) {
			Debug.Log ("left");
		} else if (target.transform.position.x > viewToWorldRight().x) {
			Debug.Log ("right");
		} else {
			Debug.Log ("on screen");
		}
	}*/


	//USING ITWEEN
	/*public void SnapToPosition(GameObject target){
		Vector3 snapPos = (target.transform.position /2f);
		StartCoroutine (LerpToPos(target, snapPos));
	}

	IEnumerator LerpToPos(GameObject target, Vector3 snapPos){
		iTween.MoveTo (gameObject, new Vector3(snapPos.x, transform.position.y, snapPos.z), 1f);
		while (transform.position != snapPos) {
			cannotMoveCamera = true;
			transform.position = Vector3.Lerp (transform.position, new Vector3 (snapPos.x, transform.position.y, snapPos.z), Time.deltaTime * panSpeed);
			yield return null;
		}
		if (target.transform.position.x < viewToWorldLeft ().x) {
			Debug.Log ("left");
			//StartCoroutine (ZoomOut(target));
		} else {
			Debug.Log ("fine");
		}
		cannotMoveCamera = false;
		yield return null;
	}*/
}
