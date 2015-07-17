using UnityEngine;
using System.Collections;

public class DemonLemming : Unit {

    public int damage = 5;

    void Update()
    {
        base.Update();
        if (this.state == UnitState.IDLE)
        {
            WallChunk w = this.FindValidWall();

            if (w != null)
            {
                this.target = w.gameObject;
                this.SetGoal(w.transform.position);
                this.SwitchToState(UnitState.MOVING);
            }
        }
    }

    //OVERRIDES
    protected override void OnSpawn()
    {
        if (this.goal.Equals(INIT_GOAL)) 
        {

            WallChunk w = this.FindValidWall();

            if (w != null)
            {
                this.target = w.gameObject;
                this.SetGoal(w.transform.position);
                this.SwitchToState(UnitState.MOVING);
            } 
            else 
            {
                this.SwitchToState(UnitState.IDLE);
            }
        } 
        else 
        {
            this.SwitchToState(UnitState.MOVING);
        }
    }

    protected override void OnMoving()
    {
        if (target == null)
        {
            WallChunk w = this.FindValidWall();
            if (w == null)
            {
                this.SetGoal(INIT_GOAL);
                this.SwitchToState(UnitState.IDLE);
            }
            else
            {
                this.target = w.gameObject;
                this.SetGoal(w.transform.position);
                this.SwitchToState(UnitState.MOVING);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter- DemonLemming");

        Building b = other.gameObject.GetComponent<Building>();
        if (b && b.GetState() != BuildingState.PREBUILD && b.GetState () != BuildingState.DESTROYING)
        {
            Debug.Log("OnTriggerEnter - Demon Lemming - Collided with Building");
            other.gameObject.GetComponent<Health>().dealDamage(this.damage, "explode");
            this.SwitchToState(UnitState.DESTROYING);
        }
    }


    protected WallChunk FindValidWall()
    {
        WallChunk wall = null;
        WallChunk[] walls = GameObject.FindObjectsOfType<WallChunk>();

        float min = float.MaxValue;

        for (int i = 0; i < walls.Length; i++)
        {
            float dist = Vector3.Distance(this.transform.position, walls[i].transform.position);
            if (dist < min)
            {
                min = dist;
				if(walls[i].GetState () != BuildingState.DESTROYING){
                	wall = walls[i];
				}
            }
        }

        return wall;
    }
    
}
