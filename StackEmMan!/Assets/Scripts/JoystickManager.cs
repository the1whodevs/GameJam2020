using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    private static JoystickManager _instance;

    private Joystick[] _joysticks; //up to 4 local players!

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            InitializeJoysticks();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (!GetJoystick(1).Assigned)
            {
                PlayerManager.GetInstance().AssignJoystickToPlayer(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            if (!GetJoystick(2).Assigned)
            {
                PlayerManager.GetInstance().AssignJoystickToPlayer(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick3Button7))
        {
            if (GetJoystick(3).Assigned)
            {
                PlayerManager.GetInstance().AssignJoystickToPlayer(3);
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick4Button7))
        {
            if (GetJoystick(4).Assigned)
            {
                PlayerManager.GetInstance().AssignJoystickToPlayer(4);
            }
        }
    }



    void InitializeJoysticks()
    {
        _joysticks = new Joystick[4];

        for (int i = 0; i < _joysticks.Length; i++)
        {
            _joysticks[i] = new Joystick(i + 1);
        }
    }

    public static JoystickManager GetInstance()
    {
        return _instance;
    }

    /// <summary>
    /// Returns the Joystick reference of the respective player number.
    /// </summary>
    /// <param name="playerNum"></param>
    /// <returns></returns>
    public Joystick GetJoystick(int joyNum)
    {
        return _joysticks[joyNum - 1];
    }

    public class Joystick
    {
        public bool Assigned = false;

        private int _joyNumber; //this SHOULD be either 1, 2, 3 or 4.

        public string fireButtonAxis { get; private set; }

        public string horizontalMoveAxis { get; private set; }
        public string verticalMoveAxis { get; private set; }

        public string horizontalLookAxis { get; private set; }
        public string verticalLookAxis { get; private set; }

        public KeyCode startButton { get; private set; }

        public Joystick(int num)
        {
            _joyNumber = num;

            fireButtonAxis = "FireJ" + num;

            horizontalMoveAxis = "HorizontalMoveJ" + num;
            verticalMoveAxis = "VerticalMoveJ" + num;

            horizontalLookAxis = "HorizontalLookJ" + num;
            verticalLookAxis = "VerticalLookJ" + num;

            /*
             * Start button keycode setup!
            */
            switch (num)
            {
                case 1:
                    startButton = KeyCode.Joystick1Button7;
                    break;
                case 2:
                    startButton = KeyCode.Joystick2Button7;
                    break;
                case 3:
                    startButton = KeyCode.Joystick3Button7;
                    break;
                case 4:
                    startButton = KeyCode.Joystick4Button7;
                    break;
                default:
                    startButton = KeyCode.None;
                    break;
            }
        }
    }
    
}
