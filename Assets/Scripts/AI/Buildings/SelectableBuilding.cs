using UnityEngine;
using System.Collections;


public class SelectableBuilding : MonoBehaviour {

	public static Building selectedBuilding;
	Building thisBuilding;

	void Start(){
		selectedBuilding = null;
		thisBuilding = this.GetComponent<Building>();
	}

    void OnMouseOver(){
		if(thisBuilding.GetState() == BuildingState.DESTROYING)
		{
	        if (Input.GetMouseButtonDown (0)) 
			{

				if(selectedBuilding != null)
				{
					selectedBuilding.SwitchToState (BuildingState.DESTROYING);
				}

				selectedBuilding = this.GetComponent<Building>();
				selectedBuilding.GetComponent<MeshRenderer>().material.color = Color.blue;
	        }
			if(selectedBuilding == this.GetComponent<Building>())
			{
				if(Input.GetMouseButtonDown (1) && UnitController.idleUnitCount > 0)
				{
					selectedBuilding.Repair();
					selectedBuilding = null;
					UnitController.idleUnitCount --;
				}
			}
		}
    }

}
