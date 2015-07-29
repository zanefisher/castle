using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]

public class Building : MonoBehaviour {

	//CONTROLLERS
	protected HandController _handController;
    protected UnitController _unitController;
	protected Renderer _renderer;
	protected Color _originalColor;
    protected Collider _collider;
    protected List<Collider> _collidedObjects = new List<Collider>();
    protected NavMeshObstacle _obstacle;

    public bool preBuilt = false;
    public bool converted = false;

    protected static int _buildingID_ = 0;

    protected int id;
    protected BuildingState state;

	// Use this for initialization
	void Start () {
		this.gameObject.layer = 2;
        this.gameObject.tag = "Building";
        this.id = _buildingID_++;
        this.state = BuildingState.PREBUILD;
        this._collider = this.GetComponent<Collider>();
        this._obstacle = this.GetComponent<NavMeshObstacle>();
        this._obstacle.enabled = false;
        this._collider.isTrigger = true;
        this._handController = GameObject.FindObjectOfType<HandController>().GetComponent<HandController>();
        this._unitController = GameObject.FindObjectOfType<UnitController>().GetComponent<UnitController>();
        this._renderer = this.GetComponent<MeshRenderer>();
        this._originalColor = this._renderer.material.color;
        if (this.preBuilt) this.SwitchToState(BuildingState.BUILDING);
	}

	
	// Update is called once per frame
	void Update () {
		switch(this.state) 
		{
			case BuildingState.PREBUILD:
				this.OnPreBuild();
				break;
			case BuildingState.BUILDING:
				this.OnBuild();
				break;
			case BuildingState.IDLE:
				this.OnIdle();
				break;
			case BuildingState.DESTROYING:
				this.OnDestroy();
				break;
			case BuildingState.CUSTOM:
				this.OnCustom();
				break;
		}
	}

    public BuildingState GetState()
    {
        return this.state;
    }

	protected virtual void OnPreBuild() {}
    protected virtual void OnBuild() {}
    protected virtual void OnIdle() {}
    protected virtual void OnDestroy() {}
    protected virtual void OnCustom() {}

    public virtual void SwitchToState(BuildingState state)
    {
        this.state = state;
        switch (this.state)
        {
        	case BuildingState.BUILDING:
        		this.SwitchToBuilding();
        		break;
            case BuildingState.IDLE:
                this.SwitchToIdle();
                break;
            case BuildingState.DESTROYING:
                this.SwitchToDestroying();
                break;
            case BuildingState.CUSTOM:
                this.SwitchToCustom();
                break;
        }
    }

    protected virtual void SwitchToBuilding() { }
    protected virtual void SwitchToIdle() 
    {
        this._obstacle.enabled = true;
    }
    protected virtual void SwitchToDestroying() 
    {
        Destroy(this.gameObject);
    }
    protected virtual void SwitchToCustom() { }


    public void StickToMouse() 
    {
    	Vector3 m = MouseController.GetMousePosition();
    	this.transform.position = new Vector3(m.x,this.transform.position.y,m.z);
    }
	
	public void Repair(){
		this.gameObject.layer = 2;
		_handController.ThrowUnitToRepair (this.gameObject);
		//this.GetComponent<Health>().health = 10;
		//this.SwitchToState (BuildingState.BUILDING);
	}

    protected virtual bool IsBuildable() 
    {
    	if (this._collidedObjects.Count == 0 && MouseController.GetMouseHit().collider.gameObject.tag == "Ground") {
    		return true;
    	} else {
    		return false;
    	}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Building"))
            this._collidedObjects.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        this._collidedObjects.Remove(other);
    }

    public void SetColor(Color color)
    {
        this._renderer.material.color = color;
    }

    public void ResetColor()
    {
        this._renderer.material.color = _originalColor;
    }

    public virtual void convertToFriendly()
    {
        if (!converted) converted = !converted;
        this.tag = "Building";
    }

    public bool checkSurroundedByWall(int angle, float range)
    {
        int startangle = 0;

        RaycastHit[] hits;
        bool found;

        while (startangle <= 360)
        {
            found = false;
            hits = Physics.RaycastAll(this.transform.position, Quaternion.Euler(0, startangle, 0) * transform.forward, range);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<WallChunk>())
                {
                    found = true;
                }
            }

            if (!found)
            {
                return false;
            }

            startangle += angle;
        }


        return true;
    }
}

public enum BuildingState
{
    PREBUILD, BUILDING, IDLE, DESTROYING, CUSTOM
}
