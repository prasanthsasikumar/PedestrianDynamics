using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    public Transform side;
    public AttachedToSide attachedToSide;

    private Vector3 newPosition;

    public enum AttachedToSide
    {
        Right = 0,
        Left = 1,
        FrontSide = 2,
        BackSide = 3
    }

    public void ScaleToParent()
    {
        side.localScale = new Vector3(this.transform.localScale.x, side.localScale.y, this.transform.localScale.z);
        if (attachedToSide == AttachedToSide.FrontSide)
        {
            newPosition = transform.position - (side.transform.up * ((transform.localScale.y / 2) + 0.2f));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
        }
        else if (attachedToSide == AttachedToSide.BackSide)
        {
            newPosition = transform.position + (side.transform.up * ((transform.localScale.y / 2) + 0.2f));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
        }
        else if (attachedToSide == AttachedToSide.Right)
        {
            side.transform.Rotate(90, 0, 0);
            newPosition = transform.position + (side.transform.up * ((transform.localScale.z / 2) + 0.2f));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
            side.transform.Rotate(90, 0, 0);
        }
        else if (attachedToSide == AttachedToSide.Left)
        {
            newPosition = transform.position + (side.transform.right * ((transform.localScale.x / 2) + 0.2f));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
        }

    }
}
