using UnityEngine;
using System.Collections;

public class selectableEnemyGroup : MonoBehaviour {

	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			EnemyController.selectedEnemyGroup = gameObject;
		}
	}
}
