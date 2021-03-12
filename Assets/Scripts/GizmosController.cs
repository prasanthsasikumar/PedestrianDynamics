using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class GizmosController : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool cntrlPressed,shiftPressed;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));       

    }    

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
       
        if(cntrlPressed)
        {
            transform.RotateAround(transform.position, curScreenPoint, 0.1f);
        }else if (shiftPressed)
        {
            transform.RotateAround(transform.position, curScreenPoint, -0.1f);
        }else
            transform.position = curPosition;

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cntrlPressed = true;

        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cntrlPressed = false;

        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftPressed = true;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftPressed = false;

        }
    }

}