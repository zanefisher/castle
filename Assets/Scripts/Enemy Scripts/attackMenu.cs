using UnityEngine;
using System.Collections;

public class attackMenu : MonoBehaviour {

	public GameObject flingMenu;
	public GameObject unitPrefab;
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

	GameObject newAttackingUnit;
	public void SpawnUnit(){
		Debug.Log ("Spawning");
		newAttackingUnit = Instantiate (unitPrefab, mousePos(), Quaternion.identity) as GameObject;
	}

	public void DragUnit(){
		Debug.Log ("dragging");
		newAttackingUnit.transform.position = mousePos();
	}
	public Vector3[] flingPath;
	public void FlingUnit(){
		Debug.Log ("flinging");
		Vector3 midPoint = ((EnemyController.selectedEnemyGroup.transform.position - basePosition.position) / 2f) + basePosition.position;
		Vector3 pathMidPoint = new Vector3(midPoint.x, midPoint.y + 5f, midPoint.z);
		//Vector3[] flingPath = new Vector3[basePosition.position, pathMidPoint, EnemyController.selectedEnemyGroup.gameObject.transform.position];
		flingPath [0] = basePosition.position;
		flingPath [1] = pathMidPoint;
		flingPath [2] = EnemyController.selectedEnemyGroup.transform.position;
		newAttackingUnit.transform.position = basePosition.position;
		iTween.MoveTo (newAttackingUnit, iTween.Hash ("path", flingPath, "time", 2f, "easetype", iTween.EaseType.easeInBack));
	}
}
