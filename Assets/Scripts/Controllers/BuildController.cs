using UnityEngine;
using System.Collections;

public class BuildController : MonoBehaviour {

	Vector3 mousePos;
	public GameObject start;
	public GameObject end;

	void Update () {
		GetMousePos ();
		HandleBuildingWalls ();	
	}


	public bool buildingWalls;
	public bool connectingTowers;
	public GameObject targetTower;
	GameObject newWall;
	public GameObject wallPrefab;
	public GameObject startTower;
	GameObject endTower;
	public GameObject endTowerPrefab;
	float wallLength;
	float distance;
	void HandleBuildingWalls (){
		if (buildingWalls){
			if(Input.GetMouseButtonDown (0)){
				//start.transform.position = mousePos;
				Destroy (startTower.GetComponent<stickToMouse>());
				//Destroy(newWall.gameObject.GetComponent<stickToMouse>());
				//Destroy (endTower.GetComponent<stickToMouse>());
				newWall = Instantiate (wallPrefab, mousePos, Quaternion.identity) as GameObject;
				endTower = Instantiate (endTowerPrefab, mousePos, Quaternion.identity) as GameObject;
			}else if(Input.GetMouseButtonUp (0)){
				endTower.transform.position = startTower.transform.position + distance * startTower.transform.forward;
				buildingWalls = false;
			}else if(Input.GetMouseButton (0)){
				//end.transform.position = mousePos;
				if(connectingTowers){
					endTower.transform.position = targetTower.transform.position;
				}else{
					endTower.transform.position = mousePos;
				}
				startTower.transform.LookAt (endTower.transform);
				endTower.transform.LookAt (startTower.transform);
				distance = Vector3.Distance (startTower.transform.position, endTower.transform.position);
				wallLength = distance;
				newWall.transform.position = startTower.transform.position + distance/2 * startTower.transform.forward;
				newWall.transform.rotation = startTower.transform.rotation;
				newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, wallLength);
			}else{
				if(connectingTowers){
					startTower.transform.position = targetTower.transform.position;
				}else{
					startTower.transform.position = mousePos;
				}
			}
				                                
		}
	}

	public void BuildingWallsTrue(){
		buildingWalls = true;
	}

	Vector3 rayPos;
	void GetMousePos(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitDist;
		if (Physics.Raycast (ray, out hitDist)) {			
			rayPos = hitDist.point;
		}
		mousePos = new Vector3 (rayPos.x, 0f, rayPos.z);
	}
}
