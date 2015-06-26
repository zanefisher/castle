using UnityEngine;
using System.Collections;

public class wallScript : MonoBehaviour {

	/*public bool isPlaced;
	public bool isFinished;
	public Vector3 startPos;
	Vector3 mousePos;
	float wallLength;

	void Update(){
		GetMousePos ();
		if (isPlaced && !isFinished) {
			gameObject.layer = 2;
			//transform.LookAt (mousePos);
			transform.LookAt (new Vector3(mousePos.x, 0f, mousePos.z));
			float distance = Vector3.Distance (startPos, mousePos);
			transform.position = startPos + distance/2 * transform.forward;
			wallLength = distance;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, wallLength);

			if(Input.GetMouseButtonDown(0)){
				Debug.Log ("finish");
				isFinished = true;
				gameObject.layer = 0;
			}
		}
	}

	void GetMousePos(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			mousePos = hitDist.point;
		}
	}*/
	
	/*void OnMouseOver(){
		if(Input.GetMouseButtonDown (1)){
			Destroy (gameObject);
		}
	}*/

	BuildController buildController;
	
	void Start(){
		buildController = GameObject.Find ("BuildController").GetComponent<BuildController> ();
	}
	
	void OnMouseOver(){
		buildController.connectingTowers = true;
		buildController.targetTower = gameObject;
		if(Input.GetMouseButtonDown (1)){
			Destroy (gameObject);
		}
	}
	
	void OnMouseExit(){
		buildController.connectingTowers = false;
	}
}
