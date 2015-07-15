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

    //public static List<Unit> totalUnits = new List<Unit>();
    //public static Queue<Unit> idleUnits = new Queue<Unit>();
    //public static Queue<Unit> attackThrowingPrepUnits = new Queue<Unit>();
    //public static Queue<Unit> wallThrowingPrepUnits = new Queue<Unit>();

    //public void addIdleUnit(Unit m)
    //{
    //    if (!idleUnits.Contains(m) && !attackThrowingPrepUnits.Contains(m) && !wallThrowingPrepUnits.Contains(m))
    //    {
    //        idleUnits.Enqueue(m);
    //    }
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    Unit m = other.GetComponent<Unit>();

    //    if (m)
    //    {
    //        this.addIdleUnit(m);
    //    }
    //}
}
