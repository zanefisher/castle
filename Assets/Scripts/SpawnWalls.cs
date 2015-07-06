using UnityEngine;
using System.Collections;

public class SpawnWalls : MonoBehaviour {

	// Use this for initializatio
	public int state=0;
	public Vector3 hitPos1;
	public Vector3 hitPos2;
	public float stepSize=1f;
	public float stepDelay=1f;
	public float wallOffsetHeight=2f;
	public float buildSpeed=1f;
	public GameObject wallChunk;
	public GameObject tower;
	public Transform gunObj;

	public float wallSegments;
	GameObject prevWall;
	void Start () {
	
	}
	
	// Update is called once per frame    public Transform gunObj;
	void Update() {

		if (Input.GetMouseButton(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Vector3 incomingVec = hit.point - gunObj.position;
				Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
				Debug.DrawLine(gunObj.position, hit.point, Color.red);
				Debug.DrawRay(hit.point, reflectVec, Color.green);
			}
		}




		if(Input.GetMouseButtonDown(0)){
		Ray cursorRay= Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit cursorRayHit= new RaycastHit();
		
			if(Physics.Raycast(cursorRay, out cursorRayHit,1000f)){
				if(state==0){
					if(cursorRayHit.collider.gameObject.tag=="tower"){
						hitPos1=cursorRayHit.collider.gameObject.transform.position;
					}
					else{
						Instantiate(tower,cursorRayHit.point,Quaternion.identity);
						hitPos1=cursorRayHit.point;
					}
					hitPos1=cursorRayHit.point;
					state=1;
					//hitPos1.y+=wallOffsetHeight;
				}
				else if(state==1){
					if(cursorRayHit.collider.gameObject.tag=="tower"){
						hitPos2=cursorRayHit.collider.gameObject.transform.position;
					}
					else{
						Instantiate(tower,cursorRayHit.point,Quaternion.identity);
						hitPos2=cursorRayHit.point;
					}
					//hitPos1.y+=wallOffsetHeight;
					//hitPos2.y+=wallOffsetHeight;
					StartCoroutine(BuildWall(hitPos1,hitPos2));
					state=0;
				}
			}
		}
	}

	IEnumerator BuildWall(Vector3 startPos, Vector3 endPos){

		Vector3 buildDirection=endPos-startPos;
		Vector3 currentBuildPos=startPos;
		Vector3 lastWallPos=startPos;


		wallSegments=Vector3.Distance(startPos,endPos)/stepSize;
		int i=0;
		while(i<=wallSegments){

			Ray theRay;
			RaycastHit rayHit;
			GameObject wall =Instantiate(wallChunk,currentBuildPos+Vector3.up*5f,Quaternion.LookRotation(buildDirection)) as GameObject;

			if (Physics.Raycast(wall.transform.position, Vector3.down, out rayHit) || Physics.Raycast(wall.transform.position, Vector3.up, out rayHit)){
				Debug.Log("hit");

				wall.transform.position=new Vector3(rayHit.point.x,rayHit.point.y+wallOffsetHeight,rayHit.point.z);

				wall.transform.rotation = Quaternion.LookRotation((lastWallPos-wall.transform.position),rayHit.normal);


				//wall.GetComponent<BuildEffect>().Build(wallOffsetHeight);

				//wall.transform.rotation=Quaternion.FromToRotation(Vector3.up, rayHit.normal);
			}

			Vector3 wallScale=wall.transform.localScale;
			wallScale.z=stepSize;
			wall.transform.localScale=wallScale;

			if(prevWall!=null){
				ReshapeCubes(wall,prevWall);
			}

			currentBuildPos+=buildDirection.normalized*stepSize;
			lastWallPos=wall.transform.position;
			prevWall=wall;

			i++;
			yield return new WaitForSeconds(stepDelay);
		}
		prevWall=null;
		yield break;
	}
	void ReshapeCubes(GameObject oldCube, GameObject newCube){
		//oldCube.GetComponent<MeshRenderer>().material.color=Color.red;
		//newCube.GetComponent<MeshRenderer>().material.color=Color.blue;
		//		Debug.Log("called");
		Mesh newCubeMesh=newCube.GetComponent<MeshFilter>().mesh;
		Mesh oldCubeMesh=oldCube.GetComponent<MeshFilter>().mesh;
		
		Vector3[] newCubeVerts = new Vector3[newCubeMesh.vertexCount];
		newCubeVerts=newCubeMesh.vertices;
		Vector3[] oldCubeVerts = new Vector3[oldCubeMesh.vertexCount]; 
		oldCubeVerts=oldCubeMesh.vertices;

		/*
		for(int i=0;i<=23;i++){
			
			if(i==2||i==8||i==21){
				oldCubeVerts[i]=oldCube.transform.InverseTransformPoint(Vector3.Lerp(oldCube.transform.TransformPoint(oldCubeVerts[i]),newCube.transform.TransformPoint(newCubeVerts[4]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[4],.5f);
			}
			else if(i==3||i==9||i==19){
				oldCubeVerts[i]=oldCube.transform.InverseTransformPoint(Vector3.Lerp(oldCube.transform.TransformPoint(oldCubeVerts[i]),newCube.transform.TransformPoint(newCubeVerts[5]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[5],.5f);
			}
			else if(i==0||i==15||i==22){
				oldCubeVerts[i]=oldCube.transform.InverseTransformPoint(Vector3.Lerp(oldCube.transform.TransformPoint(oldCubeVerts[i]),newCube.transform.TransformPoint(newCubeVerts[6]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[6],.5f);
			}
			else if(i==1||i==13||i==16){
				oldCubeVerts[i]=oldCube.transform.InverseTransformPoint(Vector3.Lerp(oldCube.transform.TransformPoint(oldCubeVerts[i]),newCube.transform.TransformPoint(newCubeVerts[7]),.5f));
				//	oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[7],.5f);
			}
			else if(i==4||i==10||i==23){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(Vector3.Lerp(newCube.transform.TransformPoint(oldCubeVerts[i]),oldCube.transform.TransformPoint(newCubeVerts[4]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[4],.5f);
			}
			else if(i==5||i==11||i==17){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(Vector3.Lerp(newCube.transform.TransformPoint(oldCubeVerts[i]),oldCube.transform.TransformPoint(newCubeVerts[5]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[5],.5f);
			}
			else if(i==6||i==12||i==20){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(Vector3.Lerp(newCube.transform.TransformPoint(oldCubeVerts[i]),oldCube.transform.TransformPoint(newCubeVerts[6]),.5f));
				//oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[6],.5f);
			}
			else if(i==7||i==14||i==18){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(Vector3.Lerp(newCube.transform.TransformPoint(oldCubeVerts[i]),oldCube.transform.TransformPoint(newCubeVerts[7]),.5f));
				//	oldCubeVerts[i]=Vector3.Lerp(oldCubeVerts[i],newCubeVerts[7],.5f);
			}
		}
		
*/
		for(int i=0;i<=23;i++){
				if(i==4||i==10||i==23){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[2]));
				//newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[2]),.5f));
				}
				else if(i==5||i==11||i==17){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[3]));
				//newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[3]),.5f));
					//newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[3],.5f);
				}
				else if(i==6||i==12||i==20){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[0]));
				//newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[0]),.5f));
					//newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[0],.5f);
				}
				else if(i==7||i==14||i==18){
				newCubeVerts[i]=newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[1]));
				//newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[1]),.5f));
					//newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[1],.5f);
				}


		}

		
		newCubeMesh.vertices=newCubeVerts;
		oldCubeMesh.vertices=oldCubeVerts;
		
		newCubeMesh.RecalculateBounds();
		oldCubeMesh.RecalculateBounds();
	}

}
