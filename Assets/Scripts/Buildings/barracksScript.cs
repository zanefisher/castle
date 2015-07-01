using UnityEngine;
using System.Collections;

public class barracksScript : MonoBehaviour {

	bool isBuilt;
	public GameObject unitPrefab;

	void Update () {
		if (!isBuilt) {
			HandleBuilding ();
		} else {
			HandleBarracksBehavior ();
		}
	}

	void HandleBarracksBehavior(){
		HandleUnitSpawning ();
	}

	public float unitSpawnTimer;
	float timer;
	void HandleUnitSpawning(){
		if (timer < unitSpawnTimer) {
			timer += Time.deltaTime;
		} else {
			SpawnUnit ();
			timer = 0;
		}
	}

	void SpawnUnit(){
		GameObject newUnit = Instantiate (unitPrefab, new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z), Quaternion.identity) as GameObject;
	}

	void HandleBuilding(){
		transform.position = GetMousePos();
		if(Input.GetMouseButtonDown (0)){
			isBuilt = true;
		}
	}

	Vector3 GetMousePos(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			return new Vector3(hitDist.point.x, 0f, hitDist.point.z);
		} else {
			return Vector3.zero;
		}
	}
}
