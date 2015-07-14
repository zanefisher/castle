using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

	public static List<GameObject> totalUnits = new List<GameObject>();
	public static List<GameObject> idleUnits = new List<GameObject>();


	public static List<GameObject> attackThrowingPrepUnits = new List<GameObject>();
	public static List<GameObject> wallThrowingPrepUnits = new List<GameObject>();

	// Use this for initialization
	void Start () {
		totalUnits = new List<GameObject> ();
		idleUnits = new List<GameObject> ();
		attackThrowingPrepUnits = new List<GameObject> ();
		wallThrowingPrepUnits = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Backspace) && totalUnits.Count > 0){
			Debug.Log ("killing unit");
			int randUnitNum = Random.Range (0, totalUnits.Count);
			totalUnits[randUnitNum].GetComponent<unitScript>().DestroyUnit();
		}
	}
}
