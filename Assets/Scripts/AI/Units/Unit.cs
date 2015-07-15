using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(NavMeshAgent))]

public class Unit : MonoBehaviour {

    protected static int _unitID_ = 0;
    protected readonly static Vector3 INIT_GOAL = new Vector3(0, -1000f, 0);

    private int id = 0;
    public NavMeshAgent agent;
    protected Vector3 goal = INIT_GOAL;
    
    protected UnitState state;

    protected GameObject target;

    public bool preSpawned = false;

    //GETTERS
    public GameObject GetTarget() { return this.target; }
    public UnitState  GetState()  { return this.state;  }
    public Vector3    GetGoal()   { return this.goal;   }

    //SETTERS
    public void SetTarget(GameObject target) { this.target = target; }
    public void SetGoal(Vector3 pos) { this.goal = pos; }

    void Start()
    {
        this.id = _unitID_++;
        this.agent = GetComponent<NavMeshAgent>();
        this.state = UnitState.SPAWNING;
    }

    public void Update()
    {
        HandleStates();
    }

    protected virtual void HandleStates()
    {
        switch (this.state)
        {
            case UnitState.SPAWNING:
                this.OnSpawn();
                break;
            case UnitState.IDLE:
                this.OnIdle();
                break;
            case UnitState.MOVING:
                this.OnMoving();
                break;
            case UnitState.CUSTOM:
                this.OnCustom();
                break;
            case UnitState.DESTROYING:
                this.OnDestroy();
                break;
        }
    }

    protected virtual void OnSpawn()
    {
        if (goal.Equals(INIT_GOAL))
        {
            this.SwitchToState(UnitState.IDLE);
        }
        else
        {
            this.SwitchToState(UnitState.MOVING);
        }
    }

    protected virtual void OnIdle()
    {

        if (!this.goal.Equals(INIT_GOAL))
        {
            this.SwitchToState(UnitState.MOVING);
        }
    }

    protected virtual void OnMoving(){}

    protected virtual void OnCustom(){}

    protected virtual void OnDestroy()
    {
        Destroy(this.gameObject);
    }



    public void SwitchToState(UnitState state)
    {
        this.state = state;
        switch (state)
        {
            case UnitState.IDLE:
                this.SwitchToIdle();
                break;
            case UnitState.MOVING:
                this.SwitchToMoving();
                break;
            case UnitState.CUSTOM:
                this.SwitchToCustom();
                break;
            case UnitState.DESTROYING:
                this.SwitchToDestroying();
                break;
        }
    }

    protected virtual void SwitchToIdle()
    {
        this.goal = INIT_GOAL;
        this.agent.SetDestination(this.transform.position);
        this.agent.ResetPath();
    }

    protected virtual void SwitchToMoving()
    {
        this.agent.destination = goal;
    }

    protected virtual void SwitchToCustom()
    {
        this.goal = INIT_GOAL;
        this.agent.SetDestination(this.transform.position);
        this.agent.ResetPath();
    }

    protected virtual void SwitchToDestroying()
    {
        this.goal = INIT_GOAL;
        this.agent.SetDestination(this.transform.position);
        this.agent.ResetPath();
    }
}

public enum UnitState
{
    SPAWNING, IDLE, MOVING, DESTROYING, CUSTOM
}
