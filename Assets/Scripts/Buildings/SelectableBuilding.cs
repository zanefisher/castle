using UnityEngine;
using System.Collections;


public class SelectableBuilding : MonoBehaviour {

	public static Building selectedBuilding;

	void Start(){
		selectedBuilding = null;
	}

    void OnMouseOver(){
        if (Input.GetMouseButtonDown (0)) 
		{

			if(selectedBuilding != null)
			{
				selectedBuilding.SwitchToState (BuildingState.DESTROYING);
			}

			Debug.Log ("selected " + this.gameObject.name);
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
