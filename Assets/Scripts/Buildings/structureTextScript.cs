using UnityEngine;
using System.Collections;

public class structureTextScript : MonoBehaviour {
	
	void Update () {
		GetComponent<TextMesh> ().text = transform.parent.GetComponent<selectableStructure> ().workingUnits.Count.ToString ();
		//transform.position = transform.parent.position;
		transform.LookAt (-Camera.main.transform.position, Vector3.up);
	}
}
