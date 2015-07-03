using UnityEngine;
using System.Collections;

public class unitScript : MonoBehaviour {

	void Start(){
		if (UnitController.totalUnits != null && UnitController.idleUnits != null) {
			UnitController.totalUnits.Add (gameObject);
			UnitController.idleUnits.Add (gameObject);
		}
	}

	public void DestroyUnit(){
		UnitController.totalUnits.Remove (gameObject);
		UnitController.idleUnits.Remove (gameObject);
		Destroy (gameObject);
	}
}
