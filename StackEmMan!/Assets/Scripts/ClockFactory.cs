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

    private int RandToIndex(float random)
    {
        if (random >= 0.0f && random <= 16.67f)
        {
            return 1;
        }
        else if (random > 16.67f && random <= 16.67f * 2)
        {
            return 2;
        }
        else if (random > 16.67f * 2 && random <= 16.67f * 3)
        {
            return 3;
        }
        else if (random > 16.67f * 3 && random <= 16.67f * 4)
        {
            return 4;
        }
        else if (random > 16.67f * 4 && random <= 16.67f * 5)
        {
            return 5;
        }
        else  //if (random > 16.67f * 5 && random <= 16.67f * 6)
        {
            return 6;
        }
    }

    public GameObject GetRandomComponent()
    {
        var rand = Random.Range(0.0f, 100.0f);

        int index = RandToIndex(rand);

        switch (index)
        {
            case 1:
                return smallHands[Random.Range(0, smallHands.Count)];
            case 2:
                return bigHands[Random.Range(0, bigHands.Count)];
            case 3:
                return bells[Random.Range(0, bells.Count)];
            case 4:
                return frames[Random.Range(0, frames.Count)];
            case 5:
                return numbers[Random.Range(0, numbers.Count)];
            case 6:
                return cogs[Random.Range(0, cogs.Count)];
            default:
                return new GameObject("Why am I here?");
        }
    }

    public GameObject GetNewClock(int numOfCogs)
    {
        int cogCount = numOfCogs < 0 ? 0 : numOfCogs;

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
    public Clock AssembleClock(GameObject smallHand, GameObject bigHand, GameObject frame, GameObject bell, GameObject numbers, params GameObject[] cogs)
    {
        List<GameObject> cogsList = new List<GameObject>();

        for (int i = 0; i < cogs.Length; i++)
        {
            cogsList.Add(cogs[i]);
        }

        Clock clock = new Clock();
        clock.SetClockComponents(cogsList, smallHand, bigHand, bell, frame, numbers);
        return clock;
    }
}
