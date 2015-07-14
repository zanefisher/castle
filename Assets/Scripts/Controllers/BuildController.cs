﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildController : MonoBehaviour
{

    private HandController _handController;
    //private MouseController _mouseController;
    private UnitController _unitController;
    private Vector3 _mousePosition;

    public bool buildingWalls;

    public GameObject towerPrefab;
    public GameObject wallPrefab;

    private BCState state;

    private WallTower _startTower;
    private bool isNewStartTower = true;
    private WallTower _endTower;
    private bool isNewEndTower = true;
    private WallChunk _newWall;

    private float _distance;
    private bool _enoughUnits;
    private float _unitsRequired;


    //BUILDWALL VARS
    public float wallSegments;
    public float stepSize = 1f;
    public float stepDelay = 1f;
    public float wallOffsetHeight = 2f;
    public float buildSpeed = 1f;
    public GameObject wallChunk;
    //public GameObject tower;
    //public Transform gunObj;
    private GameObject prevWall;
    public LayerMask wallLayerMask;
    public List<GameObject> wallSegmentList;



    void Start()
    {
        _handController = GameObject.FindObjectOfType<HandController>().GetComponent<HandController>();
        //_mouseController = GameObject.FindObjectOfType<MouseController>().GetComponent<MouseController>();
        _unitController = GameObject.FindObjectOfType<UnitController>().GetComponent<UnitController>();
        wallSegmentList = new List<GameObject>();
    }

    void Update()
    {
        GetMousePos();
        if (this.state != BCState.IDLE) HandleBuildingWalls();
    }

    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitDist;
        if (Physics.Raycast(ray, out hitDist))
        {
            _mousePosition = new Vector3(hitDist.point.x, 0f, hitDist.point.z);
        }
    }

    void HandleBuildingWalls()
    {
        switch (this.state)
        {
            case BCState.STARTTOWER:
                this.OnStartTower();
                break;
            case BCState.ENDTOWER:
                this.OnEndTower();
                break;
        }
    }

    private void OnStartTower()
    {
        this._startTower.StickToMouse();
        if (Input.GetMouseButtonDown(1)) { this.SwitchToState(BCState.DESTROYING); }

        if (Input.GetMouseButtonDown(0))
        {
            WallTower t = _startTower.GetCollidingTower();
            Debug.Log("Start Tower? : " + t);
            if (t) {
                Destroy(_startTower);
                _startTower = t; 
                isNewStartTower = false; 
            }
            this.SwitchToState(BCState.ENDTOWER);
        }
    }

    private void OnEndTower()
    {
        this._endTower.StickToMouse();
        if (Input.GetMouseButtonDown(1)) { this.SwitchToState(BCState.DESTROYING); }

        if (Input.GetMouseButtonUp(0))
        {
            if (_enoughUnits) 
            {
                WallTower t = _endTower.GetCollidingTower();
                if (t)
                {
                    Destroy(_endTower);
                    _endTower = t;
                    isNewEndTower = false;
                }

                this.SwitchToState(BCState.BUILDING);
            }
            else
            {
                this.SwitchToState(BCState.DESTROYING);
            }
            
        }
        else
        {
            _startTower.transform.LookAt(_endTower.transform);
            _endTower.transform.LookAt(_startTower.transform);
            _distance = Vector3.Distance(_startTower.transform.position, _endTower.transform.position);

            _newWall.transform.position = _startTower.transform.position + _distance / 2 * _startTower.transform.forward;
            _newWall.transform.rotation = _startTower.transform.rotation;
            _newWall.transform.localScale = new Vector3(_newWall.transform.localScale.x, _newWall.transform.localScale.y, _distance);

            _unitsRequired = _distance / stepSize;


            if (_unitsRequired > UnitController.idleUnits.Count)
            {
                _enoughUnits = false;
                _newWall.SetColor(Color.red);
                _startTower.SetColor(Color.red);
                _endTower.SetColor(Color.red);
            }
            else
            {
                _enoughUnits = true;
                _newWall.SetColor(Color.blue);
                _startTower.SetColor(Color.blue);
                _endTower.SetColor(Color.blue);
            }
        }
    }


    public void SwitchToState(BCState state)
    {
        this.state = state;

        switch (this.state)
        {
            case BCState.IDLE:
                this.SwitchToIdle();
                break;
            case BCState.DESTROYING:
                this.SwitchToDestroying();
                break;
            case BCState.STARTTOWER:
                this.SwitchToStartTower();
                break;
            case BCState.ENDTOWER:
                this.SwitchToEndTower();
                break;
            case BCState.BUILDING:
                this.SwitchToBuilding();
                break;
        }
    }

    private void SwitchToIdle() { }
    private void SwitchToDestroying()
    {
        if (isNewStartTower) { Destroy(this._startTower);  }
        else { _startTower.ResetColor(); }

        if (isNewEndTower) { Destroy(this._endTower); }
        else { _endTower.ResetColor(); }

        Destroy(this._newWall);
        this.SwitchToState(BCState.IDLE);
    }

    private void SwitchToStartTower() 
    {
        _startTower = (Instantiate(towerPrefab, MouseController.GetMousePosition(), Quaternion.identity) as GameObject).GetComponent<WallTower>();
    }
    private void SwitchToEndTower() 
    {
        this._endTower = (Instantiate(towerPrefab, MouseController.GetMousePosition(), Quaternion.identity) as GameObject).GetComponent<WallTower>();
        this._newWall = (Instantiate(wallPrefab, MouseController.GetMousePosition(), Quaternion.identity) as GameObject).GetComponent<WallChunk>();
    }
    private void SwitchToBuilding() 
    {
        Destroy(_newWall);
        this._startTower.ResetColor();
        this._startTower.SwitchToState(BuildingState.IDLE);
        this._endTower.ResetColor();
        this._endTower.SwitchToState(BuildingState.IDLE);
        for (int i = 0; i <= _unitsRequired; i++)
        {
            UnitController.wallThrowingPrepUnits.Add(UnitController.idleUnits[0]);
            UnitController.idleUnits.Remove(UnitController.idleUnits[0]);
        }

        StartCoroutine(BuildWall(_startTower.gameObject, _endTower.gameObject));
        this.SwitchToState(BCState.IDLE);
    }

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
        _handController.ThrowUnitToWall(wallSegmentList);
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

    public enum BCState
    {
        IDLE, DESTROYING, STARTTOWER, ENDTOWER, BUILDING
    }
}




