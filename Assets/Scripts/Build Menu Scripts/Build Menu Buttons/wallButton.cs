using UnityEngine;
using System.Collections;

public class wallButton : MonoBehaviour {

	public GameObject wallPrefab;
	public GameObject smallTowerPrefab;
	Vector3 targetPoint;
	Vector3 spawnPos;

	public BuildController buildController;

	void Start(){
		buildController = GameObject.Find ("BuildController").GetComponent<BuildController> ();
	}

	void Update(){
		//Hotkey (yes, it does overlap with movement)
		if(Input.GetKeyDown (KeyCode.W)){
			SpawnWall();
		}
	}


	public void SpawnWall(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			targetPoint = hitDist.point;
		}
		spawnPos = new Vector3 (targetPoint.x, 0f, targetPoint.z);
		//GameObject newWall = Instantiate (wallPrefab, spawnPos, Quaternion.identity) as GameObject;
		//buildController.newWall = newWall;
		GameObject newStartTower = Instantiate (smallTowerPrefab, spawnPos, Quaternion.identity) as GameObject;
		buildController.startTower = newStartTower;
		//GameObject newEndTower = Instantiate (smallTowerPrefab, spawnPos, Quaternion.identity) as GameObject;
		//buildController.endTower = newEndTower;
		buildController.Invoke ("BuildingWallsTrue", 0.1f);
	}
}
