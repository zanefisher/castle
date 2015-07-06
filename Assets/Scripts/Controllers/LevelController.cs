using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public pathNode nodePrefab;
	public GameObject pathPrefab;
	float angleDegrees;

	void Start () {
		//angleDegrees = Random.Range (0f, 360f);
		SpawnNodes ();
		//Debug.Log (angleDegrees);
	}

	float distance;
	Vector3 midpoint;
	pathNode newNode;
	void SpawnNodes(){
		for (int i = 0; i < 4; i++) {
			newNode = Instantiate (nodePrefab, GetSpawnPos (), Quaternion.identity) as pathNode;
			//newNode.angleDegrees = angleDegrees;
			newNode.transform.LookAt (transform.position);
			Vector3 heading = newNode.transform.position - transform.position;
			distance = heading.magnitude;
			SpawnPath();
			/*Vector3 direction = heading / distance;
			Debug.Log (distance);
			Debug.Log (direction);
			Debug.Log (heading);*/
		}
	}

	void SpawnPath(){
		float pathLength = distance;
		Debug.Log (pathLength);
		Vector3 pathSpawnPos = newNode.transform.position + distance / 2f * newNode.transform.forward;
		GameObject newPath = Instantiate (pathPrefab, pathSpawnPos, Quaternion.identity) as GameObject;
		newPath.transform.LookAt (transform.position);
		newPath.transform.localScale = new Vector3 (newPath.transform.localScale.x, newPath.transform.localScale.y, pathLength);
	}

	//This works, but I like being able to get an angle, because it helps with spawning the next set of nodes
	/*Vector3 GetSpawnPos(){
		float spawnMagnitude = Random.Range (10f, 15f);
		float spawnMagnitude2 = Mathf.Pow(spawnMagnitude, 2f);
		float x2 = Random.Range (0, spawnMagnitude2);
		float z2 = spawnMagnitude2 - x2;
		float x = Mathf.Pow (x2, 0.5f);
		float z = Mathf.Pow (z2, 0.5f);
		return new Vector3 (GetPositiveOrNegative (x), 0f, GetPositiveOrNegative (z));
	}*/

	Vector3 GetSpawnPos(){
		//Initialize variables
		float x = 0f;
		float z = 0f;
		float angleRadians = 0f;
		Vector3 spawnPos;
		//float angleVariance = Random.Range (80f, 100f);
		float angleVariance = 90f;
		angleDegrees += angleVariance;

		//angleDegrees = Random.Range (0f, 360f);
		//Debug.Log (angleDegrees);
		float spawnRadius = Random.Range (10f, 15f);

		//Convert degrees to radians
		angleRadians = angleDegrees * Mathf.PI / 180f;

		//Get dimensional coordinates
		x = spawnRadius * Mathf.Cos (angleRadians);
		z = spawnRadius * Mathf.Sin (angleRadians);

		spawnPos = new Vector3 (x, 0f, z);
		return spawnPos;
	}

	float GetPositiveOrNegative(float input){
		float rand = Random.Range (0f, 1f);
		if (rand >= 0.5f) {
			return input;
		} else {
			return -input;
		}
	}
}
