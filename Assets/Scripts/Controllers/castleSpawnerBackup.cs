using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class castleSpawnerBackup : MonoBehaviour {

	public GameObject castlePrefab;
	public GameObject castlePathNodePrefab;
	public GameObject castlePathPrefab;
	public GameObject startCastle;
	public int numberOfCastles;
	public float distanceBetweenCastles;
	public float levelSize;
	List<Vector3> castlePositions;
	List<GameObject> castlePathNodes;
	List<GameObject> connectingPaths;

	void Start () {
		castlePositions = new List<Vector3> ();
		castlePathNodes = new List<GameObject> ();
		connectingPaths = new List<GameObject>();
		castlePositions.Add (Vector3.zero);
		SpawnCastlePathNodes (startCastle.transform.position, startCastle);
		SpawnCastles ();
		CleanUpNodes ();
		ConnectNodes ();
		DestroyDuplicates ();
		CleanUpFinalNodes ();
		Debug.Log (castlePathNodes.Count);
	}

	Vector3 currentCastlePosition;
	void SpawnCastles(){
		for (int i = 0; i < numberOfCastles; i ++) {
			currentCastlePosition = GetRandomPoint ();
			int listLength = castlePositions.Count;
			for(int j = 0; j < listLength ; j++){
				if(Vector3.Distance (currentCastlePosition, castlePositions[j]) >= distanceBetweenCastles){
					if(j == listLength - 1){
						SpawnCastle();
						castlePositions.Add (currentCastlePosition);
					}
				}else{
					i--;
					break;
				}
			}
		}
	}

	void SpawnCastle(){
		GameObject newCastle = Instantiate (castlePrefab, currentCastlePosition, Quaternion.identity) as GameObject;
		SpawnCastlePathNodes (currentCastlePosition, newCastle);
	}

	void SpawnCastlePathNodes(Vector3 castlePos, GameObject parentCastle){
		//Spawn top node
		GameObject newCastlePathNodeTop = Instantiate (castlePathNodePrefab, GetCastlePathNodePosition (Vector3.forward), Quaternion.identity) as GameObject;
		newCastlePathNodeTop.gameObject.transform.LookAt (castlePos);
		newCastlePathNodeTop.gameObject.transform.parent = parentCastle.transform;
		castlePathNodes.Add (newCastlePathNodeTop);
		SpawnCastlePath (newCastlePathNodeTop.gameObject);

		//Spawn right node
		GameObject newCastlePathNodeRight = Instantiate (castlePathNodePrefab, GetCastlePathNodePosition (Vector3.right), Quaternion.identity) as GameObject;
		newCastlePathNodeRight.transform.LookAt (castlePos);
		newCastlePathNodeRight.transform.parent = parentCastle.transform;
		castlePathNodes.Add (newCastlePathNodeRight);
		SpawnCastlePath (newCastlePathNodeRight.gameObject);

		//Spawn left node
		GameObject newCastlePathNodeLeft = Instantiate (castlePathNodePrefab, GetCastlePathNodePosition (Vector3.left), Quaternion.identity) as GameObject;
		newCastlePathNodeLeft.transform.LookAt (castlePos);
		newCastlePathNodeLeft.transform.parent = parentCastle.transform;
		castlePathNodes.Add (newCastlePathNodeLeft);
		SpawnCastlePath (newCastlePathNodeLeft.gameObject);

		//Spawn bottom node
		GameObject newCastlePathNodeBottom = Instantiate (castlePathNodePrefab, GetCastlePathNodePosition (Vector3.back), Quaternion.identity) as GameObject;
		newCastlePathNodeBottom.transform.LookAt (castlePos);
		newCastlePathNodeBottom.transform.parent = parentCastle.transform;
		castlePathNodes.Add (newCastlePathNodeBottom);
		SpawnCastlePath (newCastlePathNodeBottom.gameObject);
	}

	void SpawnCastlePath(GameObject castlePathNode){
		float pathLength = 10f;
		Vector3 spawnPos = castlePathNode.transform.position + castlePathNode.transform.forward * pathLength / 2;
		GameObject newCastlePath = Instantiate (castlePathPrefab, spawnPos, Quaternion.identity) as GameObject;
		newCastlePath.transform.localScale = new Vector3 (newCastlePath.transform.localScale.x, newCastlePath.transform.localScale.y, pathLength);
		newCastlePath.transform.rotation = castlePathNode.transform.rotation;
		castlePathNode.GetComponent<pathNode>().castlePath = newCastlePath;
		//newCastlePath.transform.parent = castlePathNode.transform;
	}

	Vector3 GetCastlePathNodePosition(Vector3 direction){
		return (currentCastlePosition + direction * 10); 
	}

	Vector3 GetRandomPoint(){
		float x = Random.Range (-levelSize, levelSize);
		float z = Random.Range (-levelSize, levelSize);
		return new Vector3 (x, 0f, z);
	}

	public GameObject rightMostNode;
	public GameObject leftMostNode;
	public GameObject topMostNode;
	public GameObject bottomMostNode;
	void CleanUpNodes(){
		rightMostNode.transform.position = new Vector3 (-100, 0, 0);
		leftMostNode.transform.position = new Vector3 (100, 0, 0);
		topMostNode.transform.position = new Vector3 (0, 0, -100);
		bottomMostNode.transform.position = new Vector3 (0, 0, 100);
		foreach (GameObject castlePathNode in castlePathNodes) {
			if(castlePathNode.transform.position.x > rightMostNode.transform.position.x){
				rightMostNode = castlePathNode;
			}
			if(castlePathNode.transform.position.x < leftMostNode.transform.position.x){
				leftMostNode = castlePathNode;
			}
			if(castlePathNode.transform.position.z > topMostNode.transform.position.z){
				topMostNode = castlePathNode;
			}
			if(castlePathNode.transform.position.z < bottomMostNode.transform.position.z){
				bottomMostNode = castlePathNode;
			}
		}
		Destroy (rightMostNode.GetComponent<pathNode>().castlePath);
		castlePathNodes.Remove (rightMostNode);
		Destroy (rightMostNode);
		Destroy (leftMostNode.GetComponent<pathNode>().castlePath);
		castlePathNodes.Remove (leftMostNode);
		Destroy (leftMostNode);
		Destroy (topMostNode.GetComponent<pathNode>().castlePath);
		castlePathNodes.Remove (topMostNode);
		Destroy (topMostNode);
		Destroy (bottomMostNode.GetComponent<pathNode>().castlePath);
		castlePathNodes.Remove (bottomMostNode);
		Destroy (bottomMostNode);
	}

	float smallestDistance;
	public GameObject closestNeighborNode;
	void ConnectNodes(){
		for (int i = 0; i < castlePathNodes.Count; i++) {
			smallestDistance = 100f;
			foreach (GameObject castlePathNode in castlePathNodes) {
				if(castlePathNodes[i].transform.parent != castlePathNode.transform.parent){
					float distance = Vector3.Distance (castlePathNodes[i].transform.position, castlePathNode.transform.position);
					if(distance < smallestDistance){
						if(castlePathNode.GetComponent<pathNode>().connectingPaths.Count == 0){							
							smallestDistance = distance;
							closestNeighborNode = castlePathNode;
						}else{
							foreach(GameObject path in castlePathNode.GetComponent<pathNode>().connectingPaths){
								if(distance < path.transform.localScale.z){
									smallestDistance = distance;
									closestNeighborNode = castlePathNode;
								}
							}
						}
					}
				}else{
				}
			}
			SpawnConnectingPath(castlePathNodes[i], closestNeighborNode);
		}
	}

	void SpawnConnectingPath(GameObject node1, GameObject node2){
		float distance = Vector3.Distance (node1.transform.position, node2.transform.position);
		node1.transform.LookAt (node2.transform.position);
		Vector3 spawnPos = node1.transform.position + node1.transform.forward * distance / 2;
		GameObject newConnectingPath = Instantiate (castlePathPrefab, spawnPos, Quaternion.identity) as GameObject;
		newConnectingPath.transform.localScale = new Vector3 (newConnectingPath.transform.localScale.x, newConnectingPath.transform.localScale.y, distance);
		newConnectingPath.transform.rotation = node1.transform.rotation;
		newConnectingPath.transform.parent = node1.transform;
		node1.GetComponent<pathNode> ().connectingPaths.Add (newConnectingPath);
		node2.GetComponent<pathNode> ().connectingPaths.Add (newConnectingPath);
	}

	List <GameObject> duplicateConnectingPaths;
	//public GameObject shortestConnectingNode;
	GameObject shortestConnectingNode;
	void DestroyDuplicates(){
		duplicateConnectingPaths = new List<GameObject>();
		foreach (GameObject castlePathNode in castlePathNodes) {
			shortestConnectingNode = new GameObject();
			shortestConnectingNode.transform.localScale = new Vector3 (0, 0, 100);
			if(castlePathNode.GetComponent<pathNode>().connectingPaths.Count == 0){
				Destroy (castlePathNode.GetComponent<pathNode>().castlePath);
				Destroy(castlePathNode);
			}else if(castlePathNode.GetComponent<pathNode>().connectingPaths.Count > 1){
				foreach(GameObject connectingPath in castlePathNode.GetComponent<pathNode>().connectingPaths){
					if(connectingPath.transform.localScale.z < shortestConnectingNode.transform.localScale.z){
						if(shortestConnectingNode.transform.localScale.z < 100){
							duplicateConnectingPaths.Add (shortestConnectingNode);
							shortestConnectingNode = connectingPath;
						}else{
							shortestConnectingNode = connectingPath;
							shortestConnectingNode.transform.localScale = connectingPath.transform.localScale;
						}
					}else{
						duplicateConnectingPaths.Add (connectingPath);
					}
				}
			}
			foreach (GameObject duplicateConnectingPath in duplicateConnectingPaths) {
				castlePathNode.GetComponent<pathNode>().connectingPaths.Remove (duplicateConnectingPath);
				Destroy (duplicateConnectingPath);
			}
		}
	}

	void CleanUpFinalNodes(){
		foreach (GameObject castlePathNode in castlePathNodes) {
			if (castlePathNode.GetComponent<pathNode> ().connectingPaths.Count == 0) {
				Destroy (castlePathNode.GetComponent<pathNode> ().castlePath);
				Destroy (castlePathNode);
			}
		}
	}
}
