using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private List<GameObject> cogs;

    [SerializeField] private GameObject smallHand;
    [SerializeField] private GameObject bigHand;
    [SerializeField] private GameObject bell;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject numbers;

    private GameObject[] clockComponents;
    
    void Start()
    {
        GameObject frontSide = new GameObject("Front Side");
        frontSide.transform.SetParent(transform);

        Instantiate(smallHand, frontSide.transform);
        Instantiate(bigHand, frontSide.transform);
        Instantiate(bell, frontSide.transform);
        Instantiate(frame, frontSide.transform);
        Instantiate(numbers, frontSide.transform);

        GameObject cogSide = new GameObject("Cog Side");
        cogSide.transform.SetParent(transform);

        for (int i = 0; i < cogs.Count; i++)
        {
            Instantiate(cogs[i], cogSide.transform);
        }

        cogSide.transform.position = frontSide.transform.position + transform.right * 0.6f;
    }

    public GameObject[] GetClockComponents()
    {
        if (clockComponents == null)
        {
            Debug.LogError("Trying to GetClockComponents but it's null!");
        }

        return clockComponents;
    }

    public void SetClockComponents(List<GameObject> clockCogs, GameObject sHand,
        GameObject bHand, GameObject clockBell, GameObject clockFrame, GameObject clockNumbers)
    {
        cogs = clockCogs;
        smallHand = sHand;
        bigHand = bHand;
        bell = clockBell;
        frame = clockFrame;
        numbers = clockNumbers;

        clockComponents = new GameObject[cogs.Count + 5];

        for (int i = 0; i < cogs.Count; i++)
        {
            clockComponents[i] = cogs[i];
        }

        clockComponents[cogs.Count] = smallHand;
        clockComponents[cogs.Count + 1] = bigHand;
        clockComponents[cogs.Count + 2] = bell;
        clockComponents[cogs.Count + 3] = frame;
        clockComponents[cogs.Count + 4] = numbers;
    }
    public static bool CompareClocks(Clock clock1, Clock clock2)
    {
        bool compare,smallHand, bigHand, numbers, frame, bell,cog = false;
        smallHand =
            (clock1.smallHand.GetComponent<ClockComponent>().Type == clock2.smallHand.GetComponent<ClockComponent>().Type) 
            && (clock1.smallHand.GetComponent<ClockComponent>().Color == clock2.smallHand.GetComponent<ClockComponent>().Color);
        
        bigHand=
            (clock1.bigHand.GetComponent<ClockComponent>().Type == clock2.bigHand.GetComponent<ClockComponent>().Type)
            && (clock1.bigHand.GetComponent<ClockComponent>().Color == clock2.bigHand.GetComponent<ClockComponent>().Color);

        numbers =
            (clock1.numbers.GetComponent<ClockComponent>().Type == clock2.numbers.GetComponent<ClockComponent>().Type)
            && (clock1.numbers.GetComponent<ClockComponent>().Color == clock2.numbers.GetComponent<ClockComponent>().Color);

        frame=
            (clock1.frame.GetComponent<ClockComponent>().Type == clock2.frame.GetComponent<ClockComponent>().Type)
            && (clock1.frame.GetComponent<ClockComponent>().Color == clock2.frame.GetComponent<ClockComponent>().Color);

        bell =
            (clock1.bell.GetComponent<ClockComponent>().Type == clock2.bell.GetComponent<ClockComponent>().Type)
            && (clock1.bell.GetComponent<ClockComponent>().Color == clock2.bell.GetComponent<ClockComponent>().Color);

        if (clock1.cogs.Count==clock2.cogs.Count)
        {
            bool[] cogsOK = new bool[clock1.cogs.Count];

            for (int i = 0; i < clock1.cogs.Count; i++)
            {
                cogsOK[i] = clock1.cogs[i].GetComponent<ClockComponent>().Type == clock2.cogs[i].GetComponent<ClockComponent>().Type;

                if (!cogsOK[i])
                {
                    cog = false;
                    break;
                }
                else
                {
                    cog = true;
                }
            } 
        }
        else
        {
            cog = false;
        }

        compare = smallHand && bigHand && numbers && frame && bell && cog;
        return compare;
    }
}
