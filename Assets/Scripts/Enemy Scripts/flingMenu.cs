//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class flingMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

//    attackMenu attackController;
//    bool isSelectable;

//    void Start(){
//        attackController = GameObject.Find ("Attack Menu").GetComponent<attackMenu> ();
//    }

//    void Update(){
//        HandleSelectable ();
//        HandleDragging ();
//        GetComponentInChildren<Text> ().text = UnitController.idleUnits.Count.ToString ();
//    }

//    void HandleSelectable(){
//        if (UnitController.idleUnits.Count > 0) {
//            isSelectable = true;
//            GetComponent<Image>().color = Color.white;
//        } else {
//            if(attackController.isThrowing == false){
//                isSelectable = false;
//                GetComponent<Image>().color = Color.gray;
//            }
//        }
//    }

//    public void OnBeginDrag(PointerEventData eventData){
//        if (isSelectable) {
//            attackController.SpawnMarker ();
//            isDragging = true;
//        }
//    }

//    bool isDragging;
//    void HandleDragging(){
//        if(isDragging && isSelectable){
//            if(Input.GetMouseButton (0)){
//                attackController.DragMarker();
//            }
//        }
//    }

//    public void OnDrag(PointerEventData eventData){
//    }

//    public void OnEndDrag(PointerEventData eventData){
//        if (isSelectable) {
//            Vector3 pointerPos = eventData.pointerCurrentRaycast.worldPosition;
//            attackController.FlingMarker (pointerPos);
//        }
//    }

//}
