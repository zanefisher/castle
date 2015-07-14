using UnityEngine;
using System.Collections;


public class HandHunter : Unit {

    public int damage = 5;

	protected override void OnSpawn() 
	{
		if (this.goal.Equals(INIT_GOAL)) 
		{
			this.target = GameObject.FindObjectOfType<HandController>().gameObject;

			if (this.target != null) 
			{
				this.SetGoal(target.transform.position);
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
			this.target = GameObject.FindObjectOfType<HandController>().gameObject;

			if (target == null) 
			{
				this.SetGoal(INIT_GOAL);
				this.SwitchToState(UnitState.IDLE);
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag.Equals("Hand") || other.gameObject.tag.Equals("Wall"))
        {
            other.gameObject.GetComponent<Health>().dealDamage(this.damage, "explode");
            this.SwitchToState(UnitState.DESTROYING);
        }
	}
}