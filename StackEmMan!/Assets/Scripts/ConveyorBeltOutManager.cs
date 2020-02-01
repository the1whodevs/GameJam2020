using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConveyorBeltOutManager : MonoBehaviour
{
    private ChallengeManager challengeManager;

    private int NumberOfClockComps;

    private float spawnInterval;
    public float SpawnInterval => spawnInterval;

    private Queue<GameObject> ConveyorBeltQueue = new Queue<GameObject>();

    private GameObject[] conveyorObjects, startingConveyorObjects;

    private bool hasStarted = false;

    public void Start()
    {
        conveyorObjects = GameObject.FindGameObjectsWithTag("ConveyorBeltOut");
        startingConveyorObjects = GameObject.FindGameObjectsWithTag("StartingBeltOut");

        for (int i = 0; i < conveyorObjects.Length; i++)
        {
            ConveyorBeltQueue.Enqueue(conveyorObjects[i]);
            conveyorObjects[i].GetComponent<ConveyorBelt>().ItemDeactivated += ConveyorBelt_ItemDeactivated;
            conveyorObjects[i].SetActive(false);
            conveyorObjects[i].GetComponent<ConveyorBelt>().IsRunning = false;
        }

        ConveyorBelt cb = conveyorObjects[0].GetComponent<ConveyorBelt>();
        spawnInterval = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().SpawnInterval;
    }

    public void StartConveyorOut(float sInterval)
    {
        spawnInterval = sInterval;

        StartCoroutine(ConveyorBeltSpawn());
        StartStartingBeltOut();
    }

    private IEnumerator ConveyorBeltSpawn()
    {
        if (ConveyorBeltQueue.Count > 0)
        {
            while (ConveyorBeltQueue.Count > 0)
            {
                GameObject o = ConveyorBeltQueue.Dequeue();
                o.SetActive(true);
                o.GetComponent<ConveyorBelt>().IsRunning = true;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    protected void ConveyorBelt_ItemDeactivated(object sender, EventArgs eventArgs)
    {
        ConveyorBelt conveyorBelt = (ConveyorBelt)sender;
        conveyorBelt.IsRunning = false;
        conveyorBelt.gameObject.SetActive(false);
        ConveyorBeltQueue.Enqueue(conveyorBelt.gameObject);
    }

    public void StartStartingBeltOut()
    {
        if (!hasStarted)
        {
            hasStarted = true;

            for (int i = 0; i < startingConveyorObjects.Length; i++)
            {
                ConveyorBeltStarting cbs = startingConveyorObjects[i].GetComponent<ConveyorBeltStarting>();
                cbs.IsRunning = true;
                startingConveyorObjects[i].SetActive(true);
            }
        }
    }
}
