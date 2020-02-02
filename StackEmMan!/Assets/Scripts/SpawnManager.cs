using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _p1Spawn, _p2Spawn;

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

        StartCoroutine(StartEverythingAfterSeconds(1));
    }

    IEnumerator StartEverythingAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartEverything();
    }

    void StartEverything()
    {
        // Spawns the first clock
        Spawn();

        // Starts the ConveyorBeltIn & ConveyorBeltInStarting
        StartCoroutine(ConveyorBeltSpawn());

        // Start the ConveyorBeltOut & ConveyorBeltOutStarting
        GameObject.Find("ConveyorBelts Out").GetComponent<ConveyorBeltOutManager>().StartConveyorOut(spawnInterval);

        // Spawns the two players
        PlayerManager.GetInstance().EnablePlayers(_p1Spawn, _p2Spawn);
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

            GameObject g = Instantiate(nextClock[i]); //, Vector3.zero, Quaternion.identity, spawnPoints[rand]
            g.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
            g.transform.SetParent(spawnPoints[rand], false);
            g.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

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
}
