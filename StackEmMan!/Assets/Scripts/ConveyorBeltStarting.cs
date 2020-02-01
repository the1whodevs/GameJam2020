using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltStarting : ConveyorBelt
{
    public override void ResetItem()
    {
        //reset position and deactivate ONLY, i.e. don't raise the deactivate event
        transform.position = StartingPosition;
        gameObject.SetActive(false);
    }
}
