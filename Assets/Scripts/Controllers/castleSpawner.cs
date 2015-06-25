using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class castleSpawner : MonoBehaviour {

	public GameObject castlePrefab;
	public pathNode castlePathNodePrefab;
	public GameObject castlePathPrefab;
	public GameObject connectingPathPrefab;
	public GameObject startCastle;
	public int numberOfCastles;
	public float distanceBetweenCastles;
	public float levelSize;
	List<Vector3> castlePositions;
	List<GameObject> castles;
	List<pathNode> castlePathNodes;
	List<GameObject> connectingPaths;

	void Start () {
		castles = new List<GameObject> ();
		castlePositions = new List<Vector3> ();
		castlePathNodes = new List<pathNode> ();
		connectingPaths = new List<GameObject>();
		castlePositions.Add (Vector3.zero);
		castles.Add (startCastle);
		SpawnCastlePathNodes (startCastle);
		SpawnCastles ();
		//CleanUpNodes ();
		ConnectNodes ();
		CleanUpFinalNodes ();
	}


	//Spawn Castle Stuff
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
					//Can this lead to an infinite loop?
					i--;
					break;
				}
			}
		}
	}

	Vector3 GetRandomPoint(){
		float x = Random.Range (-levelSize, levelSize);
		float z = Random.Range (-levelSize, levelSize);
		return new Vector3 (x, 0f, z);
	}

	void SpawnCastle(){
		GameObject newCastle = Instantiate (castlePrefab, currentCastlePosition, Quaternion.identity) as GameObject;
		//SpawnCastlePathNodes (currentCastlePosition, newCastle);
		SpawnCastlePathNodes (newCastle);
		castles.Add (newCastle);
	}
	//End of Spawn Castle Stuff

	//After Spawning castles, spawn the nodes that they connect to
	//Each castle has four nodes. As of right now, they are all at right angles. This could be changed pretty easily though
	void SpawnCastlePathNodes(GameObject parentCastle){
		for(int i = 0; i < 4; i++){
			Vector3 spawnPos = parentCastle.transform.position + GetCastlePathNodeSpawnAngle ();
			pathNode newCastlePathNode = Instantiate (castlePathNodePrefab, spawnPos, Quaternion.identity) as pathNode;
			newCastlePathNode.transform.LookAt (parentCastle.transform.position);
			newCastlePathNode.transform.parent = parentCastle.transform;
			castlePathNodes.Add (newCastlePathNode);
			SpawnCastlePath (newCastlePathNode);
		}
	}
	
	float angleDegrees;
	Vector3 GetCastlePathNodeSpawnAngle(){
		//Initialize variables
		float x = 0f;
		float z = 0f;
		float angleRadians = 0f;
		Vector3 spawnPos;
		float angleVariance = 90f;

		angleDegrees += angleVariance;

		float spawnRadius = 10f;
		
		//Convert degrees to radians
		angleRadians = angleDegrees * Mathf.PI / 180f;
		
		//Get dimensional coordinates
		x = spawnRadius * Mathf.Cos (angleRadians);
		z = spawnRadius * Mathf.Sin (angleRadians);
		
		spawnPos = new Vector3 (x, 0f, z);
		return spawnPos;
	}
	//End Spawning node stuff	

	//Start Spawning each castle's paths stuff
	void SpawnCastlePath(pathNode castlePathNode){
		float pathLength = 10f;
		Vector3 spawnPos = castlePathNode.transform.position + castlePathNode.transform.forward * pathLength / 2;
		GameObject newCastlePath = Instantiate (castlePathPrefab, spawnPos, Quaternion.identity) as GameObject;
		newCastlePath.transform.localScale = new Vector3 (newCastlePath.transform.localScale.x, newCastlePath.transform.localScale.y, pathLength);
		newCastlePath.transform.rotation = castlePathNode.transform.rotation;
		castlePathNode.castlePath = newCastlePath;
		//newCastlePath.transform.parent = castlePathNode.transform;
	}
	//End Spawning castle paths stuff

	//Start Connecting Nodes & spawning connecting paths
	public GameObject closestNeighborNode;
	public GameObject closestFriendlyNode;
	void ConnectNodes(){
		foreach (GameObject castle in castles) {
			float shortestDistance = 100f;
			if(castle != startCastle){
				foreach(Transform startCastleNode in startCastle.transform){
					foreach(Transform otherCastleNode in castle.transform){
						float distance = Vector3.Distance (startCastleNode.position, otherCastleNode.position);
						if(distance < shortestDistance){
							shortestDistance = distance;
							closestNeighborNode = otherCastleNode.gameObject;
							closestFriendlyNode = startCastleNode.gameObject;
						}
					}
				}
			}
			SpawnConnectingPath (closestFriendlyNode, closestNeighborNode);
		}
	}

	void SpawnConnectingPath(GameObject node1, GameObject node2){
		float distance = Vector3.Distance (node1.transform.position, node2.transform.position);
		node1.transform.LookAt (node2.transform.position);
		Vector3 spawnPos = node1.transform.position + node1.transform.forward * distance / 2;
		GameObject newConnectingPath = Instantiate (connectingPathPrefab, spawnPos, Quaternion.identity) as GameObject;
		newConnectingPath.transform.localScale = new Vector3 (newConnectingPath.transform.localScale.x, newConnectingPath.transform.localScale.y, distance);
		newConnectingPath.transform.rotation = node1.transform.rotation;
		node1.GetComponent<pathNode>().connectingPaths.Add (newConnectingPath);
		node2.GetComponent<pathNode>().connectingPaths.Add (newConnectingPath);
	}
	//End connecting nodes and spawning connecting paths stuff

	//Get rid of any nodes that don't connect to anything (and their castle paths)
	void CleanUpFinalNodes(){
		foreach (pathNode castlePathNode in castlePathNodes) {
			if (castlePathNode.connectingPaths.Count == 0) {
				Destroy (castlePathNode.castlePath);
				Destroy (castlePathNode);
			}
		}
	}

	//This function removes the outermost nodes from any future path-generation functions
	//This is useful if we have the outer nodes look for nodes to connect to
	//However, since the startCastleNodes are the only ones looking for connections, it's useless now	
	void CleanUpNodes(){
		pathNode rightMostNode = new pathNode();
		pathNode leftMostNode = new pathNode();
		pathNode topMostNode = new pathNode ();
		pathNode bottomMostNode = new pathNode ();
		rightMostNode.transform.position = new Vector3 (-100, 0, 0);
		leftMostNode.transform.position = new Vector3 (100, 0, 0);
		topMostNode.transform.position = new Vector3 (0, 0, -100);
		bottomMostNode.transform.position = new Vector3 (0, 0, 100);
		foreach (pathNode castlePathNode in castlePathNodes) {
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
		Destroy (rightMostNode.castlePath);
		castlePathNodes.Remove (rightMostNode);
		Destroy (rightMostNode);
		Destroy (leftMostNode.castlePath);
		castlePathNodes.Remove (leftMostNode);
		Destroy (leftMostNode);
		Destroy (topMostNode.castlePath);
		castlePathNodes.Remove (topMostNode);
		Destroy (topMostNode);
		Destroy (bottomMostNode.castlePath);
		castlePathNodes.Remove (bottomMostNode);
		Destroy (bottomMostNode);
	}
}


