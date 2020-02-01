using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    int currentIndex = 0;

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

    /// <summary>
    /// Make sure there is a next clock before calling this!
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetNextClock()
    {
        List<GameObject> nextClock = ClockCompsNeeded[currentIndex];
        currentIndex++;
        return nextClock;
    }

    public void FillWithClockComps()
    {
        for (int i = 0; i < ClocksNeeded.Count; i++)
        {
            List<GameObject> clockList=new List<GameObject>();

            GameObject[] comps = ClocksNeeded[i].GetComponent<Clock>().GetClockComponents();

            for (int j = 0; j < comps.Length; j++)
            {
                clockList.Add(comps[j]);
            }

            for (int j = 0; j < numberOfExtraComponents; j++)
            {
                clockList.Add(ClockFactory.instance.GetRandomComponent());
            }

            ClockCompsNeeded.Add(clockList);
        }
    }
}
