using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    public Transform side;
    public AttachedToSide attachedToSide;
    public float gap = 0.1f;

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
            newPosition = transform.position - (side.transform.up * ((transform.localScale.y / 2) + gap));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
        }
        else if (attachedToSide == AttachedToSide.BackSide)
        {
            newPosition = transform.position + (side.transform.up * ((transform.localScale.y / 2) + gap));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
        }
        else if (attachedToSide == AttachedToSide.Right)
        {
            side.transform.Rotate(90, 0, 0);
            newPosition = transform.position - (side.transform.forward * ((transform.localScale.z / 2) + gap));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
            side.transform.Rotate(90, 0, 0);
            side.transform.localScale = new Vector3(side.transform.localScale.x, side.transform.localScale.y, transform.localScale.y);
        }
        else if (attachedToSide == AttachedToSide.Left)
        {
            side.transform.Rotate(90, 0, 0);
            newPosition = transform.position + (side.transform.forward * ((transform.localScale.z / 2) + gap));
            side.transform.SetPositionAndRotation(newPosition, this.transform.rotation);
            side.transform.Rotate(90, 0, 0);
            side.transform.localScale = new Vector3(side.transform.localScale.x, side.transform.localScale.y, transform.localScale.y);
        }

    }
}
