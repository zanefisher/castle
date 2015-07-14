//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class BuildController : MonoBehaviour {

//    private HandController _handController;
//    private Vector3 _mousePosition;

//    public bool buildingWalls;
    
//    void Start()
//    {
//        _handController = GameObject.FindObjectOfType<HandController>().GetComponent<HandController>();
//    }

//    void Update()
//    {
//        GetMousePos();
//        if (buildingWalls) HandleBuildingWalls();
//    }

//    void GetMousePos()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hitDist;
//        if (Physics.Raycast(ray, out hitDist))
//        {
//            _mousePosition = new Vector3(hitDist.point.x, 0f, hitDist.point.z);
//        }
//    }

//    void HandleBuildingWalls()
//    {
        
//    }
//}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildController : MonoBehaviour
{

    Vector3 mousePos;
    public GameObject start;
    public GameObject end;
    HandController handController;
    public List<GameObject> wallSegmentList;

    void Start()
    {
        //handController = GameObject.Find ("Base").GetComponent<HandController>();
        handController = GameObject.FindObjectOfType<HandController>().GetComponent<HandController>();
        wallSegmentList = new List<GameObject>();
    }

    void Update()
    {
        GetMousePos();
        HandleBuildingWalls();
    }


    public bool buildingWalls;
    public bool connectingTowers;
    public GameObject targetTower;
    GameObject newWall;
    public GameObject wallTemplatePrefab;
    public GameObject startTower;
    GameObject endTower;
    public GameObject endTowerPrefab;
    float wallLength;
    float distance;
    float unitsRequired;
    bool notEnoughUnits;

    void HandleBuildingWalls()
    {
        if (buildingWalls)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //start.transform.position = mousePos;
                Destroy(startTower.GetComponent<stickToMouse>());
                //Destroy(newWall.gameObject.GetComponent<stickToMouse>());
                //Destroy (endTower.GetComponent<stickToMouse>());
                newWall = Instantiate(wallTemplatePrefab, mousePos, Quaternion.identity) as GameObject;
                endTower = Instantiate(endTowerPrefab, mousePos, Quaternion.identity) as GameObject;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!notEnoughUnits)
                {
                    for (int i = 0; i <= unitsRequired; i++)
                    {
                        UnitController.wallThrowingPrepUnits.Add(UnitController.idleUnits[0]);
                        UnitController.idleUnits.Remove(UnitController.idleUnits[0]);
                    }
                    endTower.transform.position = startTower.transform.position + distance * startTower.transform.forward;
                    Destroy(newWall);
                    StartCoroutine(BuildWall(startTower, endTower));
                    buildingWalls = false;
                }
                else
                {
                    Destroy(startTower);
                    Destroy(endTower);
                    Destroy(newWall);
                    buildingWalls = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                //end.transform.position = mousePos;
                if (connectingTowers)
                {
                    endTower.transform.position = targetTower.transform.position;
                }
                else
                {
                    endTower.transform.position = mousePos;
                }
                startTower.transform.LookAt(endTower.transform);
                endTower.transform.LookAt(startTower.transform);
                distance = Vector3.Distance(startTower.transform.position, endTower.transform.position);
                wallLength = distance;
                newWall.transform.position = startTower.transform.position + distance / 2 * startTower.transform.forward;
                newWall.transform.rotation = startTower.transform.rotation;
                newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, wallLength);
                unitsRequired = wallLength / stepSize;
                if (unitsRequired > UnitController.idleUnits.Count)
                {
                    notEnoughUnits = true;
                    newWall.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    notEnoughUnits = false;
                    newWall.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
            else
            {
                if (connectingTowers)
                {
                    startTower.transform.position = targetTower.transform.position;
                }
                else
                {
                    startTower.transform.position = mousePos;
                }
            }

        }
    }

    public void BuildingWallsTrue()
    {
        buildingWalls = true;
    }

    Vector3 rayPos;
    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitDist;
        if (Physics.Raycast(ray, out hitDist))
        {
            rayPos = hitDist.point;
        }
        mousePos = new Vector3(rayPos.x, 0f, rayPos.z);
    }

    public float wallSegments;
    public float stepSize = 1f;
    public float stepDelay = 1f;
    public float wallOffsetHeight = 2f;
    public float buildSpeed = 1f;
    public GameObject wallChunk;
    //public GameObject tower;
    //public Transform gunObj;
    GameObject prevWall;
    public LayerMask wallLayerMask;

    IEnumerator BuildWall(GameObject startTower, GameObject endTower)
    {

        Wall newWall = (new GameObject()).AddComponent<Wall>();
        newWall.name = "Wall";

        newWall.SetTowers(startTower.GetComponent<WallTower>(), endTower.GetComponent<WallTower>());

        Vector3 startPos = startTower.transform.position;
        Vector3 endPos = endTower.transform.position;


        Vector3 buildDirection = endPos - startPos;
        Vector3 currentBuildPos = startPos;
        Vector3 lastChunkPos = startPos;




        wallSegments = Vector3.Distance(startPos, endPos) / stepSize;
        int i = 0;
        while (i <= wallSegments)
        {

            Ray theRay;
            RaycastHit rayHit;
            GameObject chunk = Instantiate(wallChunk, currentBuildPos + Vector3.up * 5f, Quaternion.LookRotation(buildDirection)) as GameObject;

            newWall.AddChunk(chunk.GetComponent<WallChunk>());

            chunk.GetComponent<Renderer>().enabled = false;
            chunk.GetComponent<BoxCollider>().enabled = false;
            if (Physics.Raycast(chunk.transform.position, Vector3.down, out rayHit, wallLayerMask) || Physics.Raycast(chunk.transform.position, Vector3.up, out rayHit, wallLayerMask))
            {
                //if(rayHit.transform.gameObject.tag != "Tower"){

                chunk.transform.position = new Vector3(rayHit.point.x, rayHit.point.y + wallOffsetHeight, rayHit.point.z);

                chunk.transform.rotation = Quaternion.LookRotation((lastChunkPos - chunk.transform.position), rayHit.normal);
                //}


                //wall.GetComponent<BuildEffect>().Build(wallOffsetHeight);

                //wall.transform.rotation=Quaternion.FromToRotation(Vector3.up, rayHit.normal);
            }

            Vector3 wallScale = chunk.transform.localScale;
            wallScale.z = stepSize;
            chunk.transform.localScale = wallScale;

            if (prevWall != null)
            {
                ReshapeCubes(chunk, prevWall);
            }

            currentBuildPos += buildDirection.normalized * stepSize;
            lastChunkPos = chunk.transform.position;
            prevWall = chunk;

            wallSegmentList.Add(chunk);
            i++;
            yield return new WaitForSeconds(stepDelay);
        }
        prevWall = null;
        handController.ThrowUnitToWall(wallSegmentList);
        //wallSegmentList.Clear ();
        yield break;
    }

    void ReshapeCubes(GameObject oldCube, GameObject newCube)
    {
        //oldCube.GetComponent<MeshRenderer>().material.color=Color.red;
        //newCube.GetComponent<MeshRenderer>().material.color=Color.blue;
        //		Debug.Log("called");
        Mesh newCubeMesh = newCube.GetComponent<MeshFilter>().mesh;
        Mesh oldCubeMesh = oldCube.GetComponent<MeshFilter>().mesh;

        Vector3[] newCubeVerts = new Vector3[newCubeMesh.vertexCount];
        newCubeVerts = newCubeMesh.vertices;
        Vector3[] oldCubeVerts = new Vector3[oldCubeMesh.vertexCount];
        oldCubeVerts = oldCubeMesh.vertices;

        for (int i = 0; i <= 23; i++)
        {
            if (i == 4 || i == 10 || i == 23)
            {
                newCubeVerts[i] = newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[2]));
                //newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[2]),.5f));
            }
            else if (i == 5 || i == 11 || i == 17)
            {
                newCubeVerts[i] = newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[3]));
                //newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[3]),.5f));
                //newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[3],.5f);
            }
            else if (i == 6 || i == 12 || i == 20)
            {
                newCubeVerts[i] = newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[0]));
                //newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[0]),.5f));
                //newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[0],.5f);
            }
            else if (i == 7 || i == 14 || i == 18)
            {
                newCubeVerts[i] = newCube.transform.InverseTransformPoint(oldCube.transform.TransformPoint(oldCubeVerts[1]));
                //newCubeVerts[i]=transform.InverseTransformPoint(Vector3.Lerp(transform.TransformPoint(newCubeVerts[i]),transform.TransformPoint(oldCubeVerts[1]),.5f));
                //newCubeVerts[i]=Vector3.Lerp(newCubeVerts[i],oldCubeVerts[1],.5f);
            }


        }

        newCubeMesh.vertices = newCubeVerts;
        oldCubeMesh.vertices = oldCubeVerts;

        newCubeMesh.RecalculateBounds();
        oldCubeMesh.RecalculateBounds();
    }
}

