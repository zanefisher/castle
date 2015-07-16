using UnityEngine;
using System.Collections;

public class Minion : Unit {


    protected override void OnIdle()
    {
        if (this.goal.Equals(INIT_GOAL))
        {
            this.goal = _unitController.transform.position;
            this.SwitchToState(UnitState.MOVING);
        }
    }

    protected override void SwitchToIdle()
    {
        base.SwitchToIdle();
        this.agent.enabled = false;
    }

    protected override void SwitchToMoving()
    {
        this.agent.enabled = true;
        base.SwitchToMoving();
    }

    protected override void SwitchToCustom()
    {
        this.agent.enabled = false;
        base.SwitchToCustom();
    }
}
