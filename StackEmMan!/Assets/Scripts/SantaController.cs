using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SantaController : MonoBehaviour
{


    public bool Running;

    public float Speed = 1.0f;

    private JoystickManager.Joystick _joystick; //the joystick that will control the specific santa, if in local play
    /*
    public override void OnEnable()
    {

        if (GameManager.GetInstance())
        {
            //_joystick = GetComponent<LocalPlayer>().GetPlayerJoystick();
        }
    }*/

    void Update()
    {
        if (GameManager.GetInstance()) //we're checking this to make sure we're intentionally not connected!
        {
            CheckInput(_joystick);
            Rotate(_joystick);

            if (Running)
            {
                float horizontal = Input.GetAxis(_joystick.horizontalMoveAxis);
                float vertical = Input.GetAxis(_joystick.verticalMoveAxis);

                Vector3 fwd = transform.GetChild(0).transform.forward * vertical;
                Vector3 rht = transform.GetChild(0).transform.right * horizontal;
                Vector3 move = fwd + rht;

                move.Normalize();

                transform.localPosition += move * Speed * Time.deltaTime;

                //_santaAnimator.Run(vertical * Speed);
            }
            else
            {
                //_santaAnimator.Run(0);
            }
        }



    }

    void CheckInput()
    {
        Running = !(Mathf.Approximately(Input.GetAxis("Horizontal"), 0) && Mathf.Approximately(Input.GetAxis("Vertical"), 0.0f));
    }

    void CheckInput(JoystickManager.Joystick localPlayerJoystick)
    {
        Running = !(Mathf.Approximately(Input.GetAxis(localPlayerJoystick.horizontalMoveAxis), 0.0f) && Mathf.Approximately(Input.GetAxis(localPlayerJoystick.verticalMoveAxis), 0.0f));
    }

    void Rotate()
    {
        RaycastHit hit;
       // Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 lookAtPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        //    transform.LookAt(lookAtPos, Vector3.up);
        //}
    }

    void Rotate(JoystickManager.Joystick localPlayerJoystick)
    {
        if (!Mathf.Approximately(Input.GetAxis(localPlayerJoystick.horizontalLookAxis), 0.0f) || !Mathf.Approximately(Input.GetAxis(localPlayerJoystick.verticalLookAxis), 0.0f))
        {
            transform.forward = new Vector3(Input.GetAxis(localPlayerJoystick.horizontalLookAxis), 0.0f, Input.GetAxis(localPlayerJoystick.verticalLookAxis));
        }
    }

}
