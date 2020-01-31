using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody _rb;


    private JoystickManager.Joystick _joystick;

    public void OnEnable()
    {
        if (GameManager.GetInstance())
        {
            _joystick = GetComponent<LocalPlayer>().GetPlayerJoystick();
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (GameManager.GetInstance())
        {
            Move();
        }
    }

    void Move()
    {
        float horizontal = Input.GetAxis(_joystick.horizontalMoveAxis);
        float vertical = Input.GetAxis(_joystick.verticalMoveAxis);

        Vector3 translation = new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * _moveSpeed;

        _rb.AddForce(translation, ForceMode.Impulse);
    }

    void Rotate()
    {
        float horizontal = Input.GetAxis(_joystick.horizontalLookAxis);
        float vertical = Input.GetAxis(_joystick.verticalLookAxis);

        Vector3 direction = new Vector3(horizontal, 0.0f, vertical);

        transform.LookAt(direction);

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
