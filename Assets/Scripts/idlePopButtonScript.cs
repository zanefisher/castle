//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;

//public class idlePopButtonScript : MonoBehaviour {

//    void Update(){
//        gameObject.GetComponentInChildren <Text> ().text = "Idle: \n" + UnitController.idleUnits.Count;
//    }

//    public void DeployIdleUnit(){
//        if (UnitController.idleUnits.Count > 0 && StructureController.selectedStructure.currentUnits < StructureController.selectedStructure.maxUnits) {
//            StructureController.selectedStructure.AddUnit (UnitController.idleUnits[0]);
//            UnitController.idleUnits.Remove (UnitController.idleUnits[0]);
//        }
//    }
//}
