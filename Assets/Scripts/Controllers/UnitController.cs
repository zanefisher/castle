using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

    public static int idleUnitCount = 0;

    void OnTriggerStay(Collider other)
    {
        Unit m = other.GetComponent<Unit>();

        if (m && m.GetState() != UnitState.SPAWNING)
        {
            idleUnitCount++;
            m.SwitchToState(UnitState.DESTROYING);
        }
    }
}
