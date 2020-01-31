using System.Collections.Generic;
using UnityEngine;

public class ClockFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> cogs;
    [SerializeField] private List<GameObject> smallHands;
    [SerializeField] private List<GameObject> bigHands;
    [SerializeField] private List<GameObject> bells;
    [SerializeField] private List<GameObject> frames;
    [SerializeField] private List<GameObject> numbers;

    public static ClockFactory instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public GameObject GetRandomComponent()
    {
        // TODO....
        return new GameObject("FIX ME!");
    }

    public GameObject GetNewClock(int totalClockComponents)
    {
        int cogCount = totalClockComponents - 5;

        Clock newClock = new Clock();

        List<GameObject> cogsToUse = new List<GameObject>();

        if (cogCount > 0)
        {
            for (int i = 0; i < cogCount; i++)
            {
                cogsToUse.Add(GetRandomCog());
            }
        }

        GameObject clock = new GameObject((Time.time * Time.deltaTime * Random.Range(0, 9)).ToString(), typeof(Clock));

        clock.GetComponent<Clock>().SetClockComponents(
                cogsToUse,
                GetRandomSmallHand(),
                GetRandomBigHand(),
                GetRandomBell(),
                GetRandomFrame(),
                GetRandomNumbers()
            );

        return clock;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Make a clock plis!"))
        {
            GetNewClock(8);
        }
    }

    private GameObject GetRandomCog()
    {
        return cogs[Random.Range(0, cogs.Count)];
    }

    private GameObject GetRandomSmallHand()
    {
        return smallHands[Random.Range(0, smallHands.Count)];
    }

    private GameObject GetRandomBigHand()
    {
        return bigHands[Random.Range(0, bigHands.Count)];
    }

    private GameObject GetRandomBell()
    {
        return bells[Random.Range(0, bells.Count)];
    }

    private GameObject GetRandomFrame()
    {
        return frames[Random.Range(0, frames.Count)];
    }

    private GameObject GetRandomNumbers()
    {
        return numbers[Random.Range(0, numbers.Count)];
    }
}
