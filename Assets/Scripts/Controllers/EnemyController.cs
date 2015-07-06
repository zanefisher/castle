using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public static GameObject selectedEnemyGroup;

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			DeselectEnemyGroup ();
		}
	}

	public void DeselectEnemyGroup(){
		selectedEnemyGroup = null;
	}

}
