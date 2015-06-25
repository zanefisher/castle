using UnityEngine;
using System.Collections;

public class checkTrigger : MonoBehaviour {

	public bool connectingTowers;

	void OnTriggerEnter(Collider col){
		if (col.tag == "Tower") {
			connectingTowers = true;
			Debug.Log ("connecting towers");
		} 
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Tower") {
			connectingTowers = false;
		}
	}
}
