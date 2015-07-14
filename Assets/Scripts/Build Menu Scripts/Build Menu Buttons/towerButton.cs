using UnityEngine;
using System.Collections;

public class towerButton : MonoBehaviour {

	public GameObject towerPrefab;
	Vector3 targetPoint;
	Vector3 spawnPos;

	public void SpawnTower(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			targetPoint = hitDist.point;
		}
		spawnPos = new Vector3 (targetPoint.x, 0f, targetPoint.z);
		GameObject newTower = Instantiate (towerPrefab, spawnPos, Quaternion.identity) as GameObject;
	}
}
