using UnityEngine;
using System.Collections;

public class DemonLemming : Unit {

    public int damage = 5;

    private GameObject target;

    //GETTERS
    public GameObject getTarget() { return this.target; }

    //SETTERS
    public void setTarget(GameObject target) { this.target = target; }

    //OVERRIDES
    protected override void OnSpawning()
    {
        if (this.goal.Equals(INIT_GOAL)) 
        {

            GameObject w = this.FindValidWall();

            if (w != null)
            {
                this.target = w;
                this.SetGoal(w.transform.position);
            }
        }

        this.SwitchToState(UnitState.MOVING);
    }

    protected override void OnMoving()
    {
        if (target == null)
        {
            GameObject w = this.FindValidWall();
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


    protected GameObject FindValidWall()
    {
        GameObject wall = null;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        float min = float.MaxValue;

        for (int i = 0; i < walls.Length; i++)
        {
            float dist = Vector3.Distance(this.transform.position, walls[i].transform.position);
            if (dist < min)
            {
                min = dist;
                wall = walls[i];
            }
        }

        return wall;
    }
    
}
