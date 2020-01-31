using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    int numOfComponenetsPerClock;
    int numOfClocks;
    List<Clock> ClocksNeeded;
    public bool CheckChallengeComplete;

    void Start()
    {
        MakeClocksNeeded();
    }

    private void MakeClocksNeeded()
    {
        for (int i = 0; i < numOfClocks; i++)
        {

        }
    }
}
