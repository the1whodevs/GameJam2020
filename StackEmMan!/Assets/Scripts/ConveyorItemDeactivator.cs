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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag(boundaryTag))
        {
            conveyorBelt.ResetItem();
        }
    }
}
