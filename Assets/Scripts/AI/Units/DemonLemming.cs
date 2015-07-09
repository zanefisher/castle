using UnityEngine;
using System.Collections;

public class DemonLemming : Unit {

    public int damage = 5;

    void Update()
    {
        base.Update();
        if (this.state == UnitState.IDLE)
        {
            GameObject w = this.FindValidWall();

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
        Debug.Log("OnSpawn");
        if (this.goal.Equals(INIT_GOAL)) 
        {

            GameObject w = this.FindValidWall();

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("WallChunk"))
        {
            other.gameObject.GetComponent<WallChunk>().dealDamage(this.damage, "explode");
            this.SwitchToState(UnitState.DESTROYING);
        }

        if (other.gameObject.tag.Equals("WallTower"))
        {
            other.gameObject.GetComponent<WallTower>().dealDamage(this.damage, "explode");
            this.SwitchToState(UnitState.DESTROYING);
        }
    }


    protected GameObject FindValidWall()
    {
        GameObject wall = null;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("WallChunk");

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
