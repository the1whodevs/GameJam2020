using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDrill : Tool
{
    [SerializeField] private int _maxBattery = 100;
    [SerializeField] private int _currentBattery;
    [SerializeField] float drillTimer = 10.0f;
    
    int itemInUse = false;

    public override void Use()
    {
        itemInUse = true;
    }  

    // Checks when the player is using the object and begin the timer
    public void Update()
    {
        if (itemInUse == true)
        {
            drillTimer -= Time.deltaTime;

            if (drillTimer < 0)
            {
                Debug.Log("Drill used for 10 seconds.");
                itemInUse = false;
            }
        }
    }
}
