using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {

	BuildController buildController;

    public float grabRange = 5f;

	void Start(){
		buildController = GameObject.Find ("BuildController").GetComponent<BuildController>();
	}

	//STRICTLY FOR TESTING THE UNIT THROWING IN THE UITESTING SCENE
	public GameObject unitPrefab;
	void Update(){
		if (Input.GetKeyDown (KeyCode.I)) {
			GameObject newUnit = Instantiate (unitPrefab, new Vector3(Random.Range (-2f,2f), 0f, Random.Range(-2f,2f)), Quaternion.identity) as GameObject;
		}
	}

	public void ThrowUnitToEnemy(Vector3 target){
		for (int i = 0; i < UnitController.attackThrowingPrepUnits.Count; i++) {
			GameObject newAttackingUnit = UnitController.attackThrowingPrepUnits [i];
			Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
			Vector3[] throwPath = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero};
			throwPath [0] = transform.position;
			throwPath [1] = midPoint;
			throwPath [2] = target;
			iTween.MoveTo (newAttackingUnit, iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad)); 
		}
		UnitController.attackThrowingPrepUnits.Clear ();
	}

	int curThrownUnits;
	int maxThrownUnits;
	public void ThrowUnitToWall(List<GameObject> wallSegmentsList){
		/*for (int i = 1; i < wallSegmentsList.Count - 1; i++){
			GameObject newWallUnit = UnitController.idleUnits [0];
			UnitController.idleUnits.Remove (newWallUnit);
			Vector3 target = wallSegmentsList[i].transform.position;
			Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
			Vector3[] throwPath = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero};
			throwPath[0] = transform.position;
			throwPath[1] = midPoint;
			throwPath[2] = target;
			//iTween.MoveTo (newWallUnit, iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "oncompleteparams", buildController.wallSegmentList[i]));
			Hashtable newWallUnitHash = iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "oncompleteparams", wallSegmentsList[i]);
			StartCoroutine (ThrowUnitToWallCoroutine (newWallUnit, newWallUnitHash));
		}*/

		//Using this throws them with a delay that's set in the coroutine. If you want them to all throw at once, comment this out and uncomment the stuff above
		StartCoroutine (ThrowToWall (wallSegmentsList));
	}

	IEnumerator ThrowToWall(List<GameObject> segments){
		int i = 0;
		int max = segments.Count;
		//I'm not sure if this has cases where it can create an infinite loop
		while(i < max){
			GameObject newWallUnit = UnitController.wallThrowingPrepUnits [0];
			UnitController.wallThrowingPrepUnits.Remove (newWallUnit);
			Vector3 target = segments[0].transform.position;
			Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
			Vector3[] throwPath = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero};
			throwPath[0] = transform.position;
			throwPath[1] = midPoint;
			throwPath[2] = target;
			//iTween.MoveTo (newWallUnit, iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "oncompleteparams", buildController.wallSegmentList[i]));
			Hashtable newWallUnitHash = iTween.Hash ("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "oncompleteparams", segments[0]);
			StartCoroutine (ThrowUnitToWallCoroutine (newWallUnit, newWallUnitHash));
			buildController.wallSegmentList.Remove(buildController.wallSegmentList[0]);
			i++;
			yield return new WaitForSeconds(0.2f);
		}
	}
	

	IEnumerator ThrowUnitToWallCoroutine(GameObject unitToThrow, Hashtable unitHash){
		iTween.MoveTo (unitToThrow, unitHash);
		yield return null;
	}

	void TurnUnitIntoWall(GameObject wallSegment){
		wallSegment.GetComponent<Renderer>().enabled = true;
		wallSegment.GetComponent<BoxCollider>().enabled = true;
	}
}
