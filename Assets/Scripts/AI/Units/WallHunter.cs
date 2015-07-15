using UnityEngine;
using System.Collections;

public class WallHunter : Unit {

    public int damage = 5;


    void Update()
    {
        if (this.state == UnitState.IDLE)
        {
            GameObject w = WallHunter.FindValidWall(this.transform.position);

            if (w != null)
            {
                this.target = w;
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

            GameObject w = WallHunter.FindValidWall(this.transform.position);
            Debug.Log(w);
            if (w != null)
            {
                this.target = w;
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
            GameObject w = WallHunter.FindValidWall(this.transform.position);
            if (w == null)
            {
                this.SetGoal(INIT_GOAL);
                this.SwitchToState(UnitState.IDLE);
            }
            else
            {
                this.target = w;
                this.SetGoal(w.transform.position);
                this.SwitchToState(UnitState.MOVING);
            }
        }

        if (!target.transform.position.Equals(goal))
        {
            this.SetGoal(target.transform.position);
            this.agent.SetDestination(this.goal);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(target))
        {
            other.gameObject.GetComponent<Health>().dealDamage(this.damage, "explode");
            this.SwitchToState(UnitState.DESTROYING);
        }
    }


    protected static GameObject FindValidWall(Vector3 pos)
    {
        GameObject wall = null;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        Debug.Log("finding wall");
        float min = float.MaxValue;

        for (int i = 0; i < walls.Length; i++)
        {
            float dist = Vector3.Distance(pos, walls[i].transform.position);
            if (dist < min)
            {
                min = dist;
                wall = walls[i];
            }
        }

        return wall;
    }
    
}
