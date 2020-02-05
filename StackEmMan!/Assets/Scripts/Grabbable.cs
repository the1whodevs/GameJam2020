using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool held = false;

    private Transform handsHoldingMe = null;

    private Collider2D _col;

    private void Start()
    {
        _col = (Collider2D)GetComponent(typeof(Collider2D));
    }

    private void Update()
    {
        if (held)
        {
            transform.position = handsHoldingMe.position;
        }   
    }

    // Returns true if the item is successfully picked up, false if not.
    public bool PickUp(Transform transformToFollow)
    {
        if (!held)
        {
            if (transform.parent)
            {
                transform.SetParent(null);
            }

            held = true;
            handsHoldingMe = transformToFollow;
            _col.enabled = false;
            return true;
        }

        return false;
    }

    public void Drop()
    {
        if (held)
        {
            held = false;
            handsHoldingMe = null;
            _col.enabled = true;
        }
    }
}
