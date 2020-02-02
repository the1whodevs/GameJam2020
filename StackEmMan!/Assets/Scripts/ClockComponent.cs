using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComponentType { smallHand, bigHand, numbers, frame, bell, smallCog, mediumCog, bigCog}
public enum ComponentColor { red, yellow, blue}

public class ClockComponent : MonoBehaviour
{
    public ComponentType Type;
    public ComponentColor Color;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        //transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    public void FixScale()
    {
        transform.localScale = initialScale;
    }
}
