//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class selectableStructure : MonoBehaviour {

//    public float maxUnits;
//    public float currentUnits;
//    public List<GameObject> workingUnits;

//    void Start(){
//        workingUnits = new List<GameObject> ();
//    }

//    void OnMouseOver(){
//        if (Input.GetMouseButtonDown (0)) {
//            StructureController.selectedStructure = this;
//            Debug.Log (StructureController.selectedStructure.gameObject.name);
//        }
//        if (Input.GetMouseButtonDown (1)) {
//            RemoveUnit ();
//        }
//    }

//    public void AddUnit(GameObject newUnit){
//        workingUnits.Add (newUnit);
//        newUnit.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y / 2f, gameObject.transform.position.z);
//        currentUnits ++;
//        Debug.Log (workingUnits.Count);
//    }

//    public void RemoveUnit(){
//        if (workingUnits.Count > 0) {
//            UnitController.idleUnits.Enqueue()
//            workingUnits.Remove (workingUnits[0]);
//            currentUnits --;
//            Debug.Log (workingUnits.Count);
//        }
//    }

//}
