using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public event EventHandler ItemDeactivated;

    public int ID;

    [SerializeField] readonly Transform[] spawnPoints = new Transform[5];
    [SerializeField] string boundaryTag;
    [SerializeField] bool isRunning = false;  //MOVING ITEMS MUST BE ENABLED BY THE SPAWNER
    [SerializeField] private float startingSpeed = 0.6f;
    [SerializeField] private float length = 5f;

    // private bool actionInProgress;

    private float currentSpeed;
    private Vector3 velocity;
    private Vector3 startingPosition;

    public Transform[] SpawnPoints => spawnPoints;

    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public float CurrentSpeed => currentSpeed;

    public float Length => length;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);

        //GUIManager.GameReset += GuiManager_GameReset;

        currentSpeed = startingSpeed;
        velocity = currentSpeed * Vector3.left;
        

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
        if (isRunning)
        {
            transform.localPosition += velocity * Time.deltaTime;
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

    #region Comparison - may not be needed in current implementation

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

    #endregion
}
