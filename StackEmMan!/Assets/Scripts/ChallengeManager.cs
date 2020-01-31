using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    int numOfComponentsPerClock;
    int numOfClocks;

    [SerializeField] private int numberOfExtraComponents;

    List<GameObject> ClocksNeeded;
    List<List<GameObject>> ClockCompsNeeded;

    void Start()
    {
        MakeClocksNeeded();
    }

    private void MakeClocksNeeded()
    {
        for (int i = 0; i < numOfClocks; i++)
        {
            ClocksNeeded.Add(ClockFactory.instance.GetNewClock(numOfComponentsPerClock));
        }
    }

    public bool CheckChallengeComplete()
    {
        return true;
    }

    public void FillWithClockComps()
    {
        // For each clock...
            // Make a list and ...
            // For each component..
                // Add the component to the clock's list
            // Then, add "numberOfExtraComponenets" in the clock's list, using ClockFactory.instance.GetRandomComponent()
            // Add the clock's list, to the "ClockCompsNeeded" list
        
    }
}
