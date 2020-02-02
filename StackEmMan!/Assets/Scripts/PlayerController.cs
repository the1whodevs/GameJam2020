using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20.0f;
    [SerializeField] private Transform _hands;
    [SerializeField] private GameObject _pickUp;

    //[SerializeField] private GameObject _tool;
    //[SerializeField] private GameObject _clock;

    private Rigidbody _rb;
    private ClockComponent _clock;
    private Tool _tool;


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

        //_rb.position += translation;
        //_rb.MovePosition(_rb.position + translation);
        //_rb.AddForce(translation, ForceMode.Acceleration);
        _rb.velocity = translation.normalized * _moveSpeed;
    }

    public void Interact()
    {
        bool isHoldingSth = false;

        if (Input.GetKeyDown(_joystick.interactButton))
        {
            Debug.Log("Player pressed the interact button");

            if (!isHoldingSth)
            {
                PickUp();
            }
            else if (isHoldingSth)
            {
                Drop();
            }


        }
    }

    public void PickUp()
    {
        _pickUp.transform.SetParent(_hands);

        if (_pickUp.GetType() == typeof(ClockComponent))
        {
            _clock = GetComponent<ClockComponent>();
        }
        else if (_pickUp.GetType() == typeof(Tool))
        {
            _tool = GetComponent<Tool>();
        }
    }

    public void Drop()
    {
        if (Input.GetKeyDown(_joystick.dropButton))
        {
            Debug.Log("Player pressed the drop button");
        }
    }

    void CheckInput()
    {
       // Running = !(Mathf.Approximately(Input.GetAxis("Horizontal"), 0) && Mathf.Approximately(Input.GetAxis("Vertical"), 0.0f));
    }

    void CheckInput(JoystickManager.Joystick localPlayerJoystick)
    {
        //Running = !(Mathf.Approximately(Input.GetAxis(localPlayerJoystick.horizontalMoveAxis), 0.0f) && Mathf.Approximately(Input.GetAxis(localPlayerJoystick.verticalMoveAxis), 0.0f));
    }

}
