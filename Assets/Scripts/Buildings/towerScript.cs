using UnityEngine;
using System.Collections;

public class towerScript : MonoBehaviour {
	
	BuildController buildController;

	void Start(){
		buildController = GameObject.Find ("BuildController").GetComponent<BuildController> ();
	}

	void OnMouseOver(){
        //buildController.connectingTowers = true;
        //buildController.targetTower = gameObject;
        //if(Input.GetMouseButtonDown (1)){
        //    Destroy (gameObject);
        //}
	}

	void OnMouseExit(){
		//buildController.connectingTowers = false;
	}

}
