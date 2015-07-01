using UnityEngine;
using System.Collections;

public class barracksButton : MonoBehaviour {

	public GameObject barracksPrefab;
	Vector3 targetPoint;
	Vector3 spawnPos;
	
	public void SpawnBarracks(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			targetPoint = hitDist.point;
		}
		spawnPos = new Vector3 (targetPoint.x, 0f, targetPoint.z);
		GameObject newTower = Instantiate (barracksPrefab, spawnPos, Quaternion.identity) as GameObject;
	}
}
