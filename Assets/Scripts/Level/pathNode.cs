using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pathNode : MonoBehaviour {

	public List<GameObject> connectingPaths;
	public GameObject castlePath;

	void Start(){
		connectingPaths = new List<GameObject> ();
	}


	/*public float angleDegrees;
	public GameObject node2Prefab;
	public GameObject pathPrefab;
	float angleVariance;
	bool angleVarianceMinus;
	bool angleVariancePlus;
	bool firstNodeSpawned;

	void Start () {
		SpawnTier2Node ();
	}


	float distance;
	GameObject newNode2;
	void SpawnTier2Node(){
		for (int i = 0; i < 2; i++) {
			newNode2 = Instantiate (node2Prefab, GetSpawnPos (), Quaternion.identity) as GameObject;
			newNode2.transform.LookAt (transform.position);
			distance = Vector3.Distance (newNode2.transform.position, transform.position);
			SpawnPath ();
			firstNodeSpawned = true;
		}
	}

	void SpawnPath(){
		float pathLength = distance;
		Debug.Log (pathLength);
		Vector3 pathSpawnPos = newNode2.transform.position + distance / 2f * newNode2.transform.forward;
		GameObject newPath = Instantiate (pathPrefab, pathSpawnPos, Quaternion.identity) as GameObject;
		newPath.transform.LookAt (transform.position);
		newPath.transform.localScale = new Vector3 (newPath.transform.localScale.x, newPath.transform.localScale.y, pathLength);
	}

	Vector3 GetSpawnPos(){
		if (!firstNodeSpawned) {
			GetAngleVariance ();
		} else {
			if(angleVarianceMinus){
				angleVariance += Random.Range (45f, 120f);
			}else{
				angleVariance -= Random.Range (60f, 120f);
			}
		}
		//float angleVariance = Random.Range (angleDegrees - 60f, angleDegrees + 60f);
		float spawnRadius = Random.Range (10f, 15f);
		float angleRadians = angleVariance * Mathf.PI / 180f;

		float x = spawnRadius * Mathf.Cos (angleRadians);
		float z = spawnRadius * Mathf.Sin (angleRadians);
		
		Vector3 spawnPos = new Vector3 (transform.position.x + x, 0f, transform.position.z + z);
		return spawnPos;
	}

	void GetAngleVariance(){
		float rand = Random.Range (0f, 1f);
		if (rand >= 0.5f) {
			angleVariance = Random.Range (angleDegrees - 75f, angleDegrees - 30f);
			angleVarianceMinus = true;
		} else {
			angleVariance = Random.Range (angleDegrees + 75f, angleDegrees + 30f);
			angleVariancePlus = true;
		}
	}*/
}
