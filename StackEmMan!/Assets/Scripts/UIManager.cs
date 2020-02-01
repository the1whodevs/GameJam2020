using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject _p1Ready;
    [SerializeField] private GameObject _p2Ready;

    [SerializeField] private GameObject _p1PressStart;
    [SerializeField] private GameObject _p2PressStart;



    void Awake()
    {
        if (!instance)
        {
            instance = this;
           
            _p1Ready.SetActive(false);
            _p2Ready.SetActive(false);

            _p1PressStart.SetActive(true);
            _p2PressStart.SetActive(true);

        }
    }

    public void EnablePlayer(int playerNum)
    {

        if (playerNum == 1)
	    {

            _p1Ready.SetActive(true);
            _p1PressStart.SetActive(false);

        }
        else if (playerNum == 2)
        {
            _p2Ready.SetActive(true);
            _p2PressStart.SetActive(false);
        }
    }
    
}
