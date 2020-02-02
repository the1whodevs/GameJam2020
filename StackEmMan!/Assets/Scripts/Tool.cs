using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Hammer,
    Screwdriver
}

public class Tool : MonoBehaviour
{
    // public event EventHandler BatteryEmpty;
    // public event EventHandler ChargingStarted;
    // public event EventHandler ChargingStopped;
    public event EventHandler ChargingComplete;

    private bool isInRange;
    public ToolType CurrentToolType;
    public float FullCharge = 500f;
    private float totalChargeUsed;

    private bool isInUse;

    //For Charging screwdriver
    [SerializeField] private float FullChargeTime = 3f; //TODO: UNDO Debug Value 3s to fully charge the screwdriver
    private float chargingTime;
    // private bool isCharging;
    // private bool chargingStarted;
    // private bool chargeComplete;
    // private bool batteryEmpty;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void FixScale()
    {
        if (transform.localScale != initialScale)
        {
            transform.localScale = initialScale;
        }
    }

    public bool HasBattery()
    {
        return totalChargeUsed < FullCharge;
    }

    public void Use()
    {
        AssemblyTable.instance.Assemble();

        //if (isInRange)
        //{
        //if (CurrentToolType == ToolType.Hammer)
        //{
        //    Debug.Log("Hammering!");
        //    AssemblyTable.instance.Assemble(); 
        //}
        //else
        //{
        //    if (HasBattery())
        //    {
        //        AssemblyTable.instance.UsingScrewdriver();
        //        totalChargeUsed += Time.deltaTime;
        //    }
        //}

        //isInUse = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyTable"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInRange && other.CompareTag("AssemblyTable"))
        {
            isInRange = false;
        }

        // if (other.CompareTag("ChargingBase") && CurrentToolType == ToolType.Screwdriver && isCharging)
        // {
        //     isCharging = false;
        //
        //     ChargingEventArgs chargingEventArgs = new ChargingEventArgs(chargeComplete);
        //
        //     OnChargingStopped(chargingEventArgs);
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChargingBase") && CurrentToolType == ToolType.Screwdriver)
        {
            chargingTime += Time.deltaTime;

            //Notify that the charging has started;
            // if (!chargingStarted)
            // {
            //     // OnChargingStarted(EventArgs.Empty);
            //
            //     chargingStarted = true;
            //     chargeComplete = false;
            // }

            if (chargingTime >= FullChargeTime)
            {
                // isCharging = false;
                // chargeComplete = true;
                chargingTime = 0f;

                // ChargingEventArgs chargingEventArgs = new ChargingEventArgs(chargeComplete);

                // OnChargingStopped(chargingEventArgs);

                OnChargingComplete(EventArgs.Empty);
            }
        }
    }

    private void OnChargingComplete(EventArgs eventArgs)
    {
        if (ChargingComplete != null)
        {
            ChargingComplete(this, eventArgs);
        }
    }


    public void StopUsing()
    {
        isInUse = false;
    }

}
