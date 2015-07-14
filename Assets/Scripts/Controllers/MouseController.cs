using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    public static Vector3 mousePosition = Vector3.zero;

    private Ray _ray;
    private RaycastHit _hit = new RaycastHit();


    void Update()
    {
        this.UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            mousePosition = _hit.point;
        }
        
    }

    public static Vector3 GetMousePosition()
    {
        return mousePosition;
    }

    public RaycastHit GetMouseHit()
    {
        this.UpdateMousePosition();
        return this._hit;
    }
}
