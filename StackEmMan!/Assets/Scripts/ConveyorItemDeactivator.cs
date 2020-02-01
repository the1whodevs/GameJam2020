using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorItemDeactivator : MonoBehaviour
{
    private ConveyorBelt conveyorBelt;
    private string boundaryTag;

    void Start()
    {
        conveyorBelt = GetComponentInParent<ConveyorBelt>();
        boundaryTag = conveyorBelt.BoundaryTag;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == boundaryTag)
        {
            //FOR TESTING ONLY
            // Debug.Log(name + " hit " + other.name);

            conveyorBelt.ResetItem();
        }
    }
}
