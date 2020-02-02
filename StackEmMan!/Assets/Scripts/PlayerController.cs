using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _hands;
    [SerializeField] private float _moveSpeed = 20.0f;

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
        Tool toolInHand = _hands.childCount > 0 ? _hands.GetChild(0).GetComponent<Tool>() : null;

        if (toolInHand && toolInHand.CurrentToolType == ToolType.Screwdriver)
        {
            if (Input.GetKey(_joystick.interactButton) && inAssemblyTableTrigger)
            {
                toolInHand.Use();
            }
        }
        else if (Input.GetKeyDown(_joystick.interactButton))
        {
            Debug.Log(name + " trying to interact!");

            if (inAssemblyTableTrigger)
            {
                Debug.Log(name + " is in assembly table trigger!");

                if (holdingItem)
                {
                    Debug.Log(name + " is in holding an item in the assembly table trigger!");

                    if (_hands.GetChild(0).GetComponent<ClockComponent>())
                    {
                        Debug.Log(name + " is in holding a ClockComponent in the assembly table trigger!");

                        // Try to place the held item on the assembly table
                        bool placedItem = AssemblyTable.instance.AddComponentToTable(_hands.GetChild(0).gameObject);

                        if (placedItem)
                        {
                            Debug.Log(name + " placed a ClockComponent on the assembly table!");
                            holdingItem = false;
                        } 
                    }
                    else
                    {
                        Debug.Log("Trying to use a tool in the Assembly Table!");
                        Tool heldTool = _hands.GetChild(0).GetComponent<Tool>();

                        switch (heldTool.CurrentToolType)
                        {
                            case ToolType.Hammer:
                                Debug.Log("Using hammer!");
                                heldTool.Use();
                                break;
                            case ToolType.Screwdriver:
                                if (heldTool.HasBattery())
                                {
                                    Debug.Log("Using screwdriver!");
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
                    Debug.Log(name + " is not holding anything!");
                    
                    RaycastHit hit;
                    Ray ray = new Ray(transform.position, transform.forward);
                    
                    if (Physics.Raycast(ray, out hit, 0.5f))
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
                            hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                            holdingItem = true;
                        }
                        else if (ccHit)
                        {
                            Debug.Log(name + " is about to pickup a ClockComponent!");

                            hit.transform.SetParent(_hands, false);

                            hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;

                            holdingItem = true;
                        }
                    }
                }
                else
                {
                    // try to place something on a table/conveyor belt (NOT assembly table)
                    //hit.collider.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }

        if (Input.GetKeyDown(_joystick.dropButton))
        {
            if (holdingItem)
            {
                Tool tool = _hands.GetChild(0).GetComponent<Tool>();
                ClockComponent cc = _hands.GetChild(0).GetComponent<ClockComponent>();

                Transform child = _hands.GetChild(0);

                child.GetComponent<BoxCollider>().enabled = true;

                _hands.DetachChildren();

                if (tool)
                {
                    tool.FixScale();
                }
                else
                {
                    cc.FixScale();
                }

                holdingItem = false;
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
