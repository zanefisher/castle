using UnityEngine;
using System.Collections;

public class pathScript : MonoBehaviour {

	Quaternion thisRotation;

	void Update(){
		//transform.rotation = thisRotation;
	}

	public void SetPathRotation(Quaternion pathRotation){
		thisRotation = pathRotation;
	}
}
