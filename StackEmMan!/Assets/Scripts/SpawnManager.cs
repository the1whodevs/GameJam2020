using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
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
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();

        conveyorObjects = GameObject.FindGameObjectsWithTag("ConveyorBelt");
        startingConveyorObjects = GameObject.FindGameObjectsWithTag("StartingBelt");

        for (int i = 0; i < conveyorObjects.Length ; i++)
        {
            ConveyorBeltQueue.Enqueue(conveyorObjects[i]);
            conveyorObjects[i].GetComponent<ConveyorBelt>().ItemDeactivated += ConveyorBelt_ItemDeactivated;
            conveyorObjects[i].SetActive(false);
            conveyorObjects[i].GetComponent<ConveyorBelt>().IsRunning = false;
        }

        ConveyorBelt cb = conveyorObjects[0].GetComponent<ConveyorBelt>();
        spawnInterval = cb.Length / cb.CurrentSpeed;
        Debug.Log(spawnInterval);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Press me plis"))
        {
            Spawn();
            StartCoroutine(ConveyorBeltSpawn());
        }
    }

    private IEnumerator ConveyorBeltSpawn()
    {
        Debug.Log("ENUM (BEFORE IF): " + ConveyorBeltQueue.Count);

        if (ConveyorBeltQueue.Count > 0)
        {
            while (ConveyorBeltQueue.Count > 0)
            {
                Debug.Log("ENUM: " + ConveyorBeltQueue.Count);
                GameObject o = ConveyorBeltQueue.Dequeue();
                o.SetActive(true);
                o.GetComponent<ConveyorBelt>().IsRunning = true;
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
        Debug.Log(ConveyorBeltQueue.Count);
    }

    public void Spawn()
    {
        List<Transform> spawnPoints = new List<Transform>();

        foreach (GameObject g in conveyorObjects)
        {
            for (int i = 0; i < g.transform.childCount; i++)
            {
                spawnPoints.Add(g.transform.GetChild(i));
                if (spawnPoints[i].childCount > 0)
                {
                    Destroy(spawnPoints[i].GetChild(0));
                }
            }
        }

        List<GameObject> nextClock = new List<GameObject>();
        nextClock = challengeManager.GetNextClock();

        for (int i = 0; i < nextClock.Count; i++)
        {
            int rand = Random.Range(0, spawnPoints.Count);

            Instantiate(nextClock[i], Vector3.zero, Quaternion.identity, spawnPoints[rand]);

            spawnPoints.RemoveAt(rand);
        
        }

        

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

    public void Despawn()
    {

    }
}
