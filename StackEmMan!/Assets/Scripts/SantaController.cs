using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class SantaController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;

    private Camera _cam;

    public bool Running;
    public bool Firing;

    public float Speed = 1.0f;

    public int Health = 100;

    public float FireRate = 1.0f;

    private float _timer = 0.0f;
    private float _fireInterval = 1.0f;

    private SantaAnimator _santaAnimator;

    private JoystickManager.Joystick _joystick; //the joystick that will control the specific santa, if in local play

    public override void OnEnable()
    {
        _santaAnimator = GetComponent<SantaAnimator>();
        _cam = GameObject.Find("Iso Camera").GetComponent<Camera>();

        if (GameManager.GetInstance().LocalPlay)
        {
            _joystick = GetComponent<LocalPlayer>().GetPlayerJoystick();
        }
    }

    void Update()
    {
        if (GameManager.GetInstance().LocalPlay) //we're checking this to make sure we're intentionally not connected!
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

                _santaAnimator.Run(vertical * Speed);
            }
            else
            {
                _santaAnimator.Run(0);
            }
        }
        else if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            CheckInput();
            Rotate();

            if (Running)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 fwd = transform.forward * vertical;
                Vector3 rht = transform.right * horizontal;
                Vector3 move = fwd + rht;

                move.Normalize();

                transform.localPosition += move * Speed * Time.deltaTime;

                _santaAnimator.Run(vertical * Speed);
            }
            else
            {
                _santaAnimator.Run(0);
            }
        }
        

        //Firing is the same regardless of online or local play
        if (Firing)
        {
            if (_timer > _fireInterval/FireRate)
            {
                _timer = 0.0f;
                Shoot();
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }


    }

    void CheckInput()
    {
        Firing = !Mathf.Approximately(Input.GetAxis("Fire1"), 0.0f);
        _santaAnimator.FireAnim(Firing);

        Running = !(Mathf.Approximately(Input.GetAxis("Horizontal"), 0) && Mathf.Approximately(Input.GetAxis("Vertical"), 0.0f));
    }

    void CheckInput(JoystickManager.Joystick localPlayerJoystick)
    {
        Firing = !Mathf.Approximately(Input.GetAxis(localPlayerJoystick.fireButtonAxis), 0.0f);
        _santaAnimator.FireAnim(Firing);

        Running = !(Mathf.Approximately(Input.GetAxis(localPlayerJoystick.horizontalMoveAxis), 0.0f) && Mathf.Approximately(Input.GetAxis(localPlayerJoystick.verticalMoveAxis), 0.0f));
    }

    void Rotate()
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookAtPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAtPos, Vector3.up);
        }
    }

    void Rotate(JoystickManager.Joystick localPlayerJoystick)
    {
        if (!Mathf.Approximately(Input.GetAxis(localPlayerJoystick.horizontalLookAxis), 0.0f) || !Mathf.Approximately(Input.GetAxis(localPlayerJoystick.verticalLookAxis), 0.0f))
        {
            transform.forward = new Vector3(Input.GetAxis(localPlayerJoystick.horizontalLookAxis), 0.0f, Input.GetAxis(localPlayerJoystick.verticalLookAxis));
        }
    }

    void Shoot()
    {
        if (GameManager.GetInstance().LocalPlay)
        {
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(_bulletPrefab.name, _firePoint.position, _firePoint.rotation);    
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
