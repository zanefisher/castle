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

    protected override void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    protected override void SwitchToBuilding()
    {
        this._renderer.material.color = this._originalColor;
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
