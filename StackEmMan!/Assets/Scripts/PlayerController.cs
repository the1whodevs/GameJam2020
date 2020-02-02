using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _hands;
    [SerializeField] private float _moveSpeed = 20.0f;

    [SerializeField] private GameObject _pickUp;

    private bool HasSubscribedToScrewdriverEvents;

    private Rigidbody _rb;

    private ClockComponent _clock;

    private Tool _tool;

    private bool holdingItem = false;
    private bool inAssemblyTableTrigger = false;

    private JoystickManager.Joystick _joystick;

    public void OnEnable()
    {
      
        _joystick = GetComponent<LocalPlayer>().GetPlayerJoystick();
        
    }

    void Start()
    {

        _rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        Move();
        Interact();
    }

    void Move()
    {
        transform.forward = new Vector3(Input.GetAxis(_joystick.horizontalMoveAxis), 0.0f, Input.GetAxis(_joystick.verticalMoveAxis));

        float horizontal = Input.GetAxis(_joystick.horizontalMoveAxis);
        float vertical = Input.GetAxis(_joystick.verticalMoveAxis);

        Vector3 translation = new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * _moveSpeed;

        _rb.velocity = translation.normalized * _moveSpeed;
    }

    public void Interact()
    {
        if (Input.GetKey(_joystick.interactButton))
        {
            if (inAssemblyTableTrigger)
            {
                if (holdingItem)
                {
                    if (_hands.GetChild(0).GetComponent<ClockComponent>())
                    {
                        // Try to place the holded item on the assembly table
                        bool placedItem = AssemblyTable.instance.AddComponentToTable(_hands.GetChild(0).gameObject);

                        if (placedItem)
                        {
                            holdingItem = false;
                        } 
                    }
                    else
                    {
                        Tool heldTool = _hands.GetChild(0).GetComponent<Tool>();

                        switch (heldTool.CurrentToolType)
                        {
                            case ToolType.Hammer:
                                heldTool.Use();
                                break;
                            case ToolType.Screwdriver:
                                if (heldTool.HasBattery())
                                {
                                    heldTool.Use(); 
                                }
                                else
                                {
                                    // TODO: Show charge needed icon!
                                    Debug.Log("Charging required!");
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                else
                {
                    // try to pick something up from the assembly table
                    if (AssemblyTable.instance.HasItemsOnTable())
                    {
                        // (use dist from each attachment point to select which object to pickup)
                        AssemblyTable.instance.GiveObjectFromTable(_hands);
                        holdingItem = true;
                    }
                    else
                    {
                        Debug.Log("No items on table!");
                    }                    
                }
            }
            else
            {
                if (!holdingItem)
                {
                    // try to pick something up
                    RaycastHit hit;
                    Ray ray = new Ray(transform.position, transform.forward);
                    
                    if (Physics.Raycast(ray, out hit, 0.375f))
                    {
                        Tool toolHit = hit.collider.gameObject.GetComponent<Tool>();
                        ClockComponent ccHit = hit.collider.gameObject.GetComponent<ClockComponent>();
                        if (toolHit)
                        {
                            if (toolHit.CurrentToolType == ToolType.Screwdriver && !HasSubscribedToScrewdriverEvents)
                            {
                                HasSubscribedToScrewdriverEvents = true;
                                toolHit.ChargingComplete += Screwdriver_ChargingComplete;
                            }

                            hit.transform.SetParent(_hands, false);
                        }
                        else if (ccHit)
                        {
                            hit.transform.SetParent(_hands, false);
                        }
                    }
                }
                else
                {
                    // try to place something on a table/conveyor belt (NOT assembly table)
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AssemblyTable"))
        {
            inAssemblyTableTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AssemblyTable"))
        {
            inAssemblyTableTrigger = false;
        }
    }

    protected void Screwdriver_ChargingComplete(object sender, EventArgs e)
    {
        // TODO: Hide "Needed charging" icon if on
        Debug.Log("Charging complete!");
    }
}
