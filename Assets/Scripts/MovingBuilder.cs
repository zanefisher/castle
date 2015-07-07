using UnityEngine;
using System.Collections;

public class MovingBuilder : MonoBehaviour {


	public int state=0;
	public Vector3 hitPos1;
	public Vector3 hitPos2;
	public float stepSize=1f;
	public float stepDelay=1f;
	public float wallOffsetHeight=2f;
	public float buildSpeed=1f;
	public GameObject wallChunk;
	
	public int wallSegments;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(0)){
			Ray cursorRay= Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit cursorRayHit= new RaycastHit();
			
			if(Physics.Raycast(cursorRay, out cursorRayHit,1000f)){
				Debug.Log("hit");
				if(state==0){
					hitPos1=cursorRayHit.point;
					state=1;
					hitPos1.y+=wallOffsetHeight;
				}
				else if(state==1){
					hitPos2=cursorRayHit.point;
					hitPos2.y+=wallOffsetHeight;
					//StartCoroutine(BuildWall(hitPos1,hitPos2));
					state=0;
				}
			}
		}
	}

	/*
	IEnumerator BuildWall(Vector3 startPos, Vector3 endPos){
		//transform.position

		Vector3 buildDirection=endPos-startPos;
		if (Physics.Raycast(currentBuildPos, Vector3.down, out rayHit)){
			float distanceToGround = rayHit.distance;
			float yPos=10f-distanceToGround;
		}
		Vector3 currentBuildPos=startPos


		while(true){
		RaycastHit rayHit;


		if (Physics.Raycast(transform.position, Vector3.down, out rayHit)){
			float distanceToGround = rayHit.distance;
			float yPos=100-distanceToGround;
		}




				yield return 0;
				}
	}
	*/
}
