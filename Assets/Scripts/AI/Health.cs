using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    protected static List<string> _damageTypes = new List<string>() { "normal", "fire", "explode" };
    public int health;

    public virtual void dealDamage(int amount, string type)
    {
        this.OnDamage(amount, type);
    }

    protected virtual void OnDamage(int amount, string type)
    {
		//Change this to switch the wall to a damaged state? Different color + selectable for repairs? Or do we just wait until the wall is destroyed?
        this.health -= amount;
        if (this.health <= 0) { this.OnDestroy(); }
    }

    protected virtual void OnDestroy()
    {	
		this.GetComponent<Building>().SwitchToState(BuildingState.DESTROYING);
        //Destroy(this.gameObject);
    }
}
