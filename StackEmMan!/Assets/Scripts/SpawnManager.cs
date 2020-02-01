using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private ChallengeManager challengeManager;

    private int NumberOfClockComps;
    
    [SerializeField] private float spawnInterval { get; set; }

    private Queue<GameObject> ConveyorBeltQueue = new Queue<GameObject>();

    private GameObject[] conveyorObjects;

    public void Awake()
    {
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();

        conveyorObjects = GameObject.FindGameObjectsWithTag("ConveyorBelt");

        for (int i = 0; i <conveyorObjects.Length ; i++)
        {
            ConveyorBeltQueue.Enqueue(conveyorObjects[i]);
            conveyorObjects[i].GetComponent<ConveyorBelt>().ItemDeactivated += ConveyorBelt_ItemDeactivated;
            conveyorObjects[i].SetActive(false);
            conveyorObjects[i].GetComponent<ConveyorBelt>().IsRunning = false;
        }
 
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Press me plis"))
        {
            Spawn();
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

            Instantiate(nextClock[i], Vector3.zero, Quaternion.identity, spawnPoints[rand]);

            spawnPoints.RemoveAt(rand);
        
        }
    }

    public void Despawn()
    {

    }
}
