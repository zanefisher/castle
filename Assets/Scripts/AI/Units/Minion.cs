using UnityEngine;
using System.Collections;

public class Minion : Unit {

    protected override void OnSpawn()
    {
        if (preSpawned)
        {
            this.SwitchToState(UnitState.IDLE);
        }
    }
	
}
