using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    int numOfComponentsPerClock;
    int numOfClocks;
    List<Clock> ClocksNeeded;
    List<GameObject> ClockCompsNeeded;
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
    public Clock[] GetClocksNeeded()
    {
        return ClocksNeeded.ToArray();
    }
    public void FillWithClockComps()
    {
        Clock[] clocks = GetClocksNeeded();
        for (int i = 0; i < clocks.Length; i++)
        {
            GameObject[] comps = clocks[i].GetClockComponents();
            for (int j = 0; j < comps.Length; j++)
            {
                ClockCompsNeeded.Add(comps[j]);
            }
        }
    }
}
