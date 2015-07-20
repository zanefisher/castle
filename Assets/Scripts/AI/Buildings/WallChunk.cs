using UnityEngine;
using System.Collections;


public class WallChunk : Building {

    public Wall parent;

    public Minion thrownMinion;


    public void SetParent(Wall p)
    {
        this.parent = p;
        this.transform.SetParent(p.transform);
    }

    protected override void OnPreBuild()
    {
        this._renderer.material.color = Color.blue;
    }

	protected override void SwitchToDestroying(){
		//Insert Destruction animation
		//Change wall chunk model to "Destroyed" model
		this._renderer.material.color = Color.green;
	}

    protected override void OnDestroy()
	{

    }

    protected override void SwitchToBuilding()
    {
        this._renderer.material.color = this._originalColor;
        this.gameObject.layer = 0;
        this.SwitchToState(BuildingState.IDLE); //SKIPS IDLE FOR NOW. ONBUILD USED FOR ANIMATIONS
    }

    public void dealDamage(int amount, string type)
    {
        this.parent.dealDamage(amount, type);
    }

    public void SetThrownMinion(Minion m)
    {
        this.thrownMinion = m;
    }

    //void OnTriggerEnter(Collider other)
    //{
        
    //    if (thrownMinion != null && other.gameObject.Equals(thrownMinion.gameObject))
    //    {
    //        Debug.Log("Trigger wallchunk");
    //        thrownMinion.SwitchToState(Unit.UnitState.DESTROYING);
    //        this.SwitchToState(BuildingState.BUILDING);
    //    }
    //}
}
