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

    private void Start()
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

        cogSide.SetActive(false);
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
}
