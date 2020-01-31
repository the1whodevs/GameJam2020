using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    [HideInInspector] public LocalPlayer _player1;
    [HideInInspector] public LocalPlayer _player2;
    [HideInInspector] public LocalPlayer _player3;
    [HideInInspector] public LocalPlayer _player4;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;

            _player1 = Player1.GetComponent<LocalPlayer>();
            _player2 = Player2.GetComponent<LocalPlayer>();
            _player3 = Player3.GetComponent<LocalPlayer>();
            _player4 = Player4.GetComponent<LocalPlayer>();

            DontDestroyOnLoad(this);
        }
    }

    public static PlayerManager GetInstance()
    {
        return _instance;
    }

    public void AssignJoystickToPlayer(int joyNum)
    {
        if (_player1.IsAvailable)
        {
            _player1.EnableAndAssignJoystick(joyNum);
            UIManager.GetInstance().EnablePlayer(1);
        }
        else if (_player2.IsAvailable)
        {
            _player2.EnableAndAssignJoystick(joyNum);
            UIManager.GetInstance().EnablePlayer(2);
        }
        else if (_player3.IsAvailable)
        {
            _player3.EnableAndAssignJoystick(joyNum);
            UIManager.GetInstance().EnablePlayer(3);
        }
        else if (_player4.IsAvailable)
        {
            _player4.EnableAndAssignJoystick(joyNum);
            UIManager.GetInstance().EnablePlayer(4);
        }
        else
        {
            Debug.LogError("No players available for joystick number " + joyNum);
        }
    }
}
