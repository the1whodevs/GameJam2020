using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEventArgs : EventArgs
{
    private bool chargeComplete;

    public ChargingEventArgs(bool chargeComplete)
    {
        this.chargeComplete = chargeComplete;
    }

    public bool ChargeComplete => chargeComplete;
}
