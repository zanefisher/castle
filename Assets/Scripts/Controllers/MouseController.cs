using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    private static Vector3 mousePosition = Vector3.zero;
    private static Ray _ray;
    private static RaycastHit _hit = new RaycastHit();


    void Update()
    {
        UpdateMousePosition();
    }

    private static void UpdateMousePosition()
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
    public static Vector3 GetFlooredMousePosition()
    {
        return new Vector3(mousePosition.x, 0, mousePosition.z);
    }

    public static RaycastHit GetMouseHit()
    {
        UpdateMousePosition();
        return _hit;
    }
}
