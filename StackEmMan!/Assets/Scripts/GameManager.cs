using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Transform _p1Spawn, _p2Spawn;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

    }
    void Start()
    {
        PlayerManager.GetInstance().EnablePlayers(_p1Spawn, _p2Spawn);
    }

    void Update()
    {
        
    }
}
