using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltStarting : ConveyorBelt
{
    //protected override void Awake()
    //{
    //    base.Awake();
    //    gameObject.SetActive(true);
    //    IsRunning = true;
    //}

    public override void ResetItem()
    {
        //reset position and deactivate ONLY, i.e. don't raise the deactivate event
        transform.position = StartingPosition;
        gameObject.SetActive(false);
    }
}
