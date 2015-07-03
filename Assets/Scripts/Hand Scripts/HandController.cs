using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

	//STRICTLY FOR TESTING THE UNIT THROWING IN THE UITESTING SCENE
	public GameObject unitPrefab;
	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			GameObject newUnit = Instantiate (unitPrefab, new Vector3(Random.Range (-2f,2f), 0f, Random.Range(-2f,2f)), Quaternion.identity) as GameObject;
		}
	}

	/*public void ThrowUnit(Vector3 target, int unitAmount){
		//Get a unit (or multiple units) from the list of idle units.
		//If you are trying to throw more than you have, just throw everything you have
		if (unitAmount > UnitController.idleUnits.Count) {
			unitAmount = UnitController.idleUnits.Count;
		}
		for (int i = 0; i < unitAmount; i++) {
			GameObject newAttackingUnit = UnitController.idleUnits [0];
			UnitController.idleUnits.Remove (newAttackingUnit);
			Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
			Vector3[] throwPath = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
			throwPath [0] = newAttackingUnit.transform.position;
			throwPath [1] = transform.position;
			throwPath [2] = midPoint;
			throwPath [3] = target;
			iTween.MoveTo (newAttackingUnit, iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad)); 
		}
	}*/

	public void ThrowUnit(Vector3 target){
		for (int i = 0; i < UnitController.throwingPrepUnits.Count; i++) {
			GameObject newAttackingUnit = UnitController.throwingPrepUnits [i];
			Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
			Vector3[] throwPath = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero};
			throwPath [0] = transform.position;
			throwPath [1] = midPoint;
			throwPath [2] = target;
			iTween.MoveTo (newAttackingUnit, iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad)); 
		}
		UnitController.throwingPrepUnits.Clear ();
	}
}
