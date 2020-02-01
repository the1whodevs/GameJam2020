using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public event EventHandler ItemDeactivated;

    public int ID;

    public float StartingSpeed;
    public static Vector3 Velocity;
    public string BoundaryTag;
    public Transform[] SpawnPoints;

    [HideInInspector]
    public bool IsRunning;  //MOVING ITEMS MUST BE ENABLED BY THE SPAWNER

    [SerializeField] private float currentSpeed;

    private bool actionInProgress;

    private Vector3 startingPosition;

    public float CurrentSpeed => currentSpeed;

    void Awake()
    {
        //TODO : Dont Destroy OnLoad

        //GUIManager.GameReset += GuiManager_GameReset;


        Velocity = StartingSpeed * Vector3.left;
        currentSpeed = StartingSpeed;

        gameObject.SetActive(false);
        
    }

    void Start()
    {
        //CurrentTransform = transform;

        //set startingPosition so that the GameObject's position can be reset to that, after it gets Deactivated
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            transform.localPosition += Velocity * Time.deltaTime;
        }
    }

    protected virtual void ResetItem()
    {
        //IsRunning = false;


        //reset position
        transform.position = startingPosition;

        //gameObject.SetActive(false);

        //raise event so that the Spawner will manage the item properly, and also deactivate active SpawnPoints and Items
        OnItemDeactivated(EventArgs.Empty);
    }

    private void OnItemDeactivated(EventArgs eventArgs)
    {
        if (ItemDeactivated != null)
        {
            ItemDeactivated(this, eventArgs);
        }
    }

    public bool Equals(ConveyorBelt other)
    {
        if (other == null)
        {
            return false;
        }

        if (ID == other.ID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        ConveyorBelt ConveyorBeltObj = obj as ConveyorBelt;

        if (ConveyorBeltObj == null)
        {
            return false;
        }
        else
        {
            return Equals(ConveyorBeltObj);
        }
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode  =. Unique ID values assigned in Inspector
        return ID.GetHashCode();
    }

    public static bool operator ==(ConveyorBelt conveyorBelt1, ConveyorBelt conveyorBelt2)
    {
        if (((object)conveyorBelt1) == null || ((object)conveyorBelt2) == null)
        {
            return Equals(conveyorBelt1, conveyorBelt2);
        }

        return conveyorBelt1.Equals(conveyorBelt2);
    }

    public static bool operator !=(ConveyorBelt conveyorBelt1, ConveyorBelt conveyorBelt2)
    {
        if (((object)conveyorBelt1) == null || ((object)conveyorBelt2) == null)
        {
            return !Equals(conveyorBelt1, conveyorBelt2);
        }

        return !(conveyorBelt1.Equals(conveyorBelt2));
    }
}
