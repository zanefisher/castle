using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class attackMenu : MonoBehaviour {

	public GameObject flingMenu;
	public GameObject unitPrefab;
	public GameObject markerPrefab;
	public GameObject markerObjectPrefab;
	public Transform basePosition;
	
	void Update () {
		HandleFlingMenu ();
	}

	void HandleFlingMenu(){
		if (EnemyController.selectedEnemyGroup != null) {
			flingMenu.SetActive (true);
		} else {
			flingMenu.SetActive (false);
		}
	}

	Vector3 targetPoint;
	Vector3 mousePos(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			return new Vector3 (hitDist.point.x, 0f, hitDist.point.z);
		} else {
			return Vector3.zero;
		}
	}

	GameObject newMarker;
	public float unitTimer;
	public int unitAmtPrev;
	public bool isThrowing;
	public void SpawnMarker(){
		isThrowing = true;
		//This is working, but spits out an error because the last newMarker was destroyed. Don't know if this can cause problems.
		newMarker = Instantiate (markerPrefab, Input.mousePosition, Quaternion.identity) as GameObject;
		newMarker.transform.parent = this.transform;
		unitTimer = 0f;
		unitAmtPrev = 0;
	}
	
	public void DragMarker(){
		newMarker.transform.position = Input.mousePosition;
		//While the mouse is down (i.e. the marker is being dragged) increase a timer that determines how many units are thrown (within reason)
		unitTimer += Time.deltaTime;
		int unitAmt = Mathf.RoundToInt (unitTimer + 0.5f);
		if (unitAmtPrev < unitAmt && UnitController.idleUnits.Count > 0) {
			GameObject newThrowingPrepUnit = UnitController.idleUnits[0];
			UnitController.attackThrowingPrepUnits.Add (newThrowingPrepUnit);
			UnitController.idleUnits.Remove (newThrowingPrepUnit);
			iTween.MoveTo (newThrowingPrepUnit, basePosition.position, 1f);
			unitAmtPrev = unitAmt;
		}
		newMarker.GetComponentInChildren<Text> ().text = UnitController.attackThrowingPrepUnits.Count.ToString ();
		/*if (unitAmt <= UnitController.idleUnits.Count) {
			newMarker.GetComponentInChildren<Text> ().text = unitAmt.ToString ();
		} else {
			newMarker.GetComponentInChildren<Text> ().text = UnitController.idleUnits.Count.ToString ();
		}*/
	}

	GameObject newMarkerObject;
	public void FlingMarker(Vector3 pointerPos){
		int unitAmount = Mathf.RoundToInt (unitTimer + 0.5f);
		//Delete the UI Marker
		Destroy (newMarker);
		//Spawn a marker GameObject
		newMarkerObject = Instantiate (markerObjectPrefab, mousePos(), Quaternion.identity) as GameObject;
		//Send a marker from the pointer to the target
		StartCoroutine ("markerTween", unitAmount);
		isThrowing = false;
	}

	IEnumerator markerTween(int unitAmount){
		Vector3 destination = EnemyController.selectedEnemyGroup.transform.position;
		iTween.MoveTo (newMarkerObject, iTween.Hash ("position", destination, "time", 1f));
		//yield return new WaitForSeconds (1f);
		//Have the hand grab a unit and throw it to the marker
		GameObject.Find ("Base").GetComponent<HandController> ().ThrowUnitToEnemy (destination/*, unitAmount*/);
		yield return null;
	}
}
