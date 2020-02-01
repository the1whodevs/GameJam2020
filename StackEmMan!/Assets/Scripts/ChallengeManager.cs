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
        for (int i = 0; i < ClocksNeeded.Count; i++)
        {
            // For each clock...
            // Make a list and ...
            List<GameObject> clockList=new List<GameObject>();
            GameObject[] comps = ClocksNeeded[i].GetComponent<Clock>().GetClockComponents();

            for (int j = 0; j < comps.Length; j++)
            {
                // For each component..
                // Add the component to the clock's list
                clockList.Add(comps[j]);
            }
            for (int j = 0; j < numberOfExtraComponents; j++)
            {
                clockList.Add(ClockFactory.instance.GetRandomComponent());
            }

            ClockCompsNeeded.Add(clockList);
        }
        
            
            // Then, add "numberOfExtraComponents" in the clock's list, using ClockFactory.instance.GetRandomComponent()
            // Add the clock's list, to the "ClockCompsNeeded" list
        
    }
}
