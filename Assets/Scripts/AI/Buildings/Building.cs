using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(Rigidbody))]

public class Building : MonoBehaviour {

	//CONTROLLERS
	protected MouseController _mouseController;
	protected HandController _handController;
	protected Renderer _renderer;
	protected Color _originalColor;
    protected Collider _collider;
    protected List<Collider> _collidedObjects = new List<Collider>();

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
        this._collider.isTrigger = true;
        this._mouseController = GameObject.FindObjectOfType<MouseController>().GetComponent<MouseController>();
        this._handController = GameObject.FindObjectOfType<HandController>().GetComponent<HandController>();
        this._renderer = this.GetComponent<MeshRenderer>();
        this._originalColor = this._renderer.material.color;
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
    protected virtual void SwitchToIdle() { }
    protected virtual void SwitchToDestroying() { }
    protected virtual void SwitchToCustom() { }


    protected void StickToMouse() 
    {
    	Vector3 m = this._mouseController.GetMousePosition();
    	this.transform.position = new Vector3(m.x,this.transform.position.y,m.z);
    }

    protected virtual bool IsBuildable() 
    {
    	if (this._collidedObjects.Count == 0 && this._mouseController.GetMouseHit().collider.gameObject.tag == "Ground") {
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
}

public enum BuildingState
{
    PREBUILD, BUILDING, IDLE, DESTROYING, CUSTOM
}
