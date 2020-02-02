using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    int currentIndex = 0;

    public static ChallengeManager instance;

    [SerializeField] private int numOfCogs;
    [SerializeField] private int numOfClocks;

    [SerializeField] private int numberOfExtraComponents;

    List<GameObject> ClocksNeeded = new List<GameObject>();
    List<List<GameObject>> ClockCompsNeeded = new List<List<GameObject>>();

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        MakeClocksNeeded();
        FillWithClockComps();
    }

    private void MakeClocksNeeded()
    {
        for (int i = 0; i < numOfClocks; i++)
        {
            ClocksNeeded.Add(ClockFactory.instance.GetNewClock(numOfCogs));
        }
    }

    public Clock GetCurrentClock()
    {
        if (currentIndex < 0)
        {
            Debug.LogError("Trying to get Current Clock but index is " + currentIndex);
        }

        return ClocksNeeded[currentIndex - 1].GetComponent<Clock>();
    }

    public bool CheckChallengeComplete(GameObject itemDelivered)
    {
        bool ClocksCorrect = Clock.CompareClocks(GetCurrentClock(), itemDelivered.GetComponent<Clock>());

        if (currentIndex==ClocksNeeded.Count-1)
        {
            //GameManager.instance();
        }
        return true;
    }

    /// <summary>
    /// Make sure there is a next clock before calling this!
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetNextClock()
    {
        List<GameObject> nextClock = new List<GameObject>();
        nextClock = ClockCompsNeeded[currentIndex];
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
