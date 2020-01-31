using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    


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
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {

    }

    void Rotate()
    {

    }


}
