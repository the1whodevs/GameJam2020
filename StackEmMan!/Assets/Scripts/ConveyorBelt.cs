using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public event EventHandler ItemDeactivated;

    [SerializeField]
    enum MoveDirection
    {
        Up,
        Down
    }

    public int ID; //TODO: consider if it's needed

    [SerializeField] readonly Transform[] spawnPoints = new Transform[5];
    [SerializeField] string boundaryTag = "Boundary";
    [SerializeField] private float startingSpeed = 0.6f;
    [SerializeField] private float length = 5f;
    [SerializeField] private MoveDirection moveDirection = MoveDirection.Down;

    bool isRunning = false;  //MOVING ITEMS MUST BE ENABLED BY THE SPAWNER
    private float currentSpeed;
    //private Vector3 velocity;
    private Vector2 velocity;
    protected Vector3 StartingPosition;

    public Transform[] SpawnPoints => spawnPoints;

    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public string BoundaryTag => boundaryTag;

    public float CurrentSpeed => currentSpeed;

    public float Length => length;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        currentSpeed = startingSpeed;

        //Set Velocity according to MOve Direction (Down: Items going IN, Up: Items going Out)
        switch (moveDirection)
        {
            case MoveDirection.Up:
                velocity = currentSpeed * Vector3.up;
            break;

            case MoveDirection.Down:
                velocity = currentSpeed * Vector3.down;
            break;

            default:
                Debug.LogError("Invalid MoveDirection"); 
            break;
        }

    }

    void Start()
    {
        //set startingPosition so that the GameObject's position can be reset to that, after it gets Deactivated
        StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            Vector2 movement = velocity * Time.deltaTime;
            transform.localPosition += new Vector3(movement.x, movement.y, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == boundaryTag)
        {
            //FOR TESTING ONLY
            Debug.Log(name + " hit " + collision.name);

            ResetItem();
        }
    }

    public virtual void ResetItem()
    {
        //reset position
        transform.position = StartingPosition;

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
