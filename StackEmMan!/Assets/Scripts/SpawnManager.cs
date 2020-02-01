using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private ChallengeManager challengeManager;
    private ConveyorBelt conveyorBelt;
    private int NumberOfClockComps;
    
    [SerializeField] private float spawnInterval { get; set; }
    [SerializeField] private Transform SpawnPoint;
    private Queue<GameObject> ConveyorBeltQueue;
    private GameObject[] conveyorObjects;
    


    public void Awake()
    {
        conveyorObjects = GameObject.FindGameObjectsWithTag("ConveyorBelt");
        for (int i = 0; i <conveyorObjects.Length ; i++)
        {
            ConveyorBeltQueue.Enqueue(conveyorObjects[i]);
            conveyorObjects[i].GetComponent<ConveyorBelt>().ItemDeactivated += ConveyorBelt_ItemDeactivated;
            conveyorObjects[i].SetActive(false);
            conveyorBelt.IsRunning = false;
        }
    }

    private IEnumerator ConveyorBeltSpawn()
    {
        if (ConveyorBeltQueue.Count > 0)
        {
            for (int i = 0; i < ConveyorBeltQueue.Count; i++)
            {
                GameObject o = ConveyorBeltQueue.Dequeue();
                o.SetActive(true);
                conveyorBelt.IsRunning = true;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        else
        {
            Debug.LogError("ConveyorBeltQueue is Empty");
        }
       
    }

    protected void ConveyorBelt_ItemDeactivated(object sender,EventArgs eventArgs)
    {
        ConveyorBelt conveyorBelt = (ConveyorBelt)sender;
        conveyorBelt.IsRunning = false;
        conveyorBelt.gameObject.SetActive(false);
        ConveyorBeltQueue.Enqueue(conveyorBelt.gameObject);
    }

    public void Spawn()
    {

    }

    public void Despawn()
    {

    }
}
