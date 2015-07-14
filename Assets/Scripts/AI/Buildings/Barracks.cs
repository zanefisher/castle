using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Health))]
public class Barracks : Building {

    public GameObject unitPrefab;
    public float spawnFrequency = 5f;
    private float _timer = 0f;
       
	protected override void OnPreBuild() {

        this.StickToMouse();

        if (this.IsBuildable())
        {
            this._renderer.material.color = Color.blue;

            if (Input.GetMouseButtonDown(0))
            {
                this.SwitchToState(BuildingState.BUILDING);
            }
        }
        else
        {
            this._renderer.material.color = Color.red;
        }

        
    }

    protected override void OnBuild()
    {  
        this.SwitchToState(BuildingState.IDLE);
    }

    protected override void OnIdle()
    {
        if (_timer < spawnFrequency)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            this.SpawnUnit();
            _timer = 0f;
        }
    }

    protected override void SwitchToBuilding()
    {
        this.gameObject.layer = 0;
        this._collider.isTrigger = false;
        this._renderer.material.color = this._originalColor;
    }


    protected void SpawnUnit()
    {
        GameObject newUnit = Instantiate(unitPrefab, this.transform.position, Quaternion.identity) as GameObject;
        Unit u = newUnit.GetComponent<Unit>();

        Vector3 goal = _handController.transform.position;
        Vector2 iuc = Random.insideUnitCircle * _handController.grabRange;

        goal += new Vector3(iuc.x, 0, iuc.y);
        u.SetGoal(goal);
    }
}
