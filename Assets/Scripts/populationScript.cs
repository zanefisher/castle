using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class populationScript : MonoBehaviour {
	
	void Update () {
		GetComponent<Text> ().text = "Population: " + UnitController.totalUnits.Count.ToString ();
	}
}
