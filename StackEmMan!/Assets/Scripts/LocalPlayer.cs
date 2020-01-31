using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public int pNum;

    public Color pNameColor;

    public bool IsAvailable = true;

    private JoystickManager.Joystick _pJoystick;

    public void EnableAndAssignJoystick(int joyNum)
    {
        _pJoystick = JoystickManager.GetInstance().GetJoystick(joyNum);
        _pJoystick.Assigned = true;
        IsAvailable = false;

        Debug.Log("Player " + pNum + " is now controlled by joystick number " + joyNum + "!");
    }

    public JoystickManager.Joystick GetPlayerJoystick()
    {
        return _pJoystick;
    }

    public void SetJoystick(JoystickManager.Joystick joy) { _pJoystick = joy; }
}
