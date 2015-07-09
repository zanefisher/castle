using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshObstacle))]

public class Building : MonoBehaviour {

    protected static int _buildingID_ = 0;

    protected int id;
    protected BuildingState state;

	// Use this for initialization
	void Start () {
        this.id = _buildingID_++;
        this.state = BuildingState.SPAWNING;
        this.GetComponent<Collider>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected virtual void OnSpawning() {}
    protected virtual void OnIdle() {}
    protected virtual void OnDestroy() {}
    protected virtual void OnCustom() {}

    public virtual void SwitchToState(BuildingState state)
    {
        this.state = state;
        switch (this.state)
        {
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

    protected virtual void SwitchToIdle() { }
    protected virtual void SwitchToDestroying() { }
    protected virtual void SwitchToCustom() { }
}

public enum BuildingState
{
    SPAWNING, IDLE, DESTROYING, CUSTOM
}
