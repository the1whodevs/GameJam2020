using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyTable : MonoBehaviour
{
    public static AssemblyTable instance;

    [SerializeField] private Transform AssemblyPoint;

    private List<GameObject> AssembledObjOnTable = new List<GameObject>();

    [SerializeField] private Transform[] AttachmentPoints;

    private bool[] attachmentPointUsed;

    public bool ClockReady = false;

    private Clock clock;

    private GameObject smallHand, bigHand, numbers, frame, bell;
    private List<GameObject> cogs;

    private int currentPriority, smallHandPriority, bigHandPriority, numbersPriority, framePriority, bellPriority, cogsPriority;

    private SpawnManager spawnManager;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;

            currentPriority = 1;
            cogsPriority = 1;
            framePriority = 1;
            numbersPriority = 2;
            smallHandPriority = 3;
            bigHandPriority = 3;
            bellPriority = 4;

            attachmentPointUsed = new bool[AttachmentPoints.Length];
        }
    }

    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    /// <summary>
    /// Called by the player, when interacting with a component in hand.
    /// </summary>
    /// <param name="obj"></param>
    public bool AddComponentToTable(GameObject obj)
    {
        ComponentType componentTypeToAdd = obj.GetComponent<ClockComponent>().Type;

        if (currentPriority==1 && ((componentTypeToAdd == ComponentType.bigCog) || (componentTypeToAdd == ComponentType.mediumCog)
            || (componentTypeToAdd == ComponentType.smallCog) || (componentTypeToAdd == ComponentType.frame)))
        {
            for (int i = 0; i < attachmentPointUsed.Length; i++)
            {
                if (!attachmentPointUsed[i])
                {
                    attachmentPointUsed[i] = true;
                    obj.transform.SetParent(AttachmentPoints[i].transform, false);
                    return true;
                }
            }
        }
        else if (currentPriority == 2 && componentTypeToAdd == ComponentType.numbers)
        {
            for (int i = 0; i < attachmentPointUsed.Length; i++)
            {
                if (!attachmentPointUsed[i])
                {
                    attachmentPointUsed[i] = true;
                    obj.transform.SetParent(AttachmentPoints[i].transform, false);
                    return true;
                }
            }
        }
        else if (currentPriority == 3 && (componentTypeToAdd == ComponentType.smallHand || componentTypeToAdd == ComponentType.bigHand))
        {
            for (int i = 0; i < attachmentPointUsed.Length; i++)
            {
                if (!attachmentPointUsed[i])
                {
                    attachmentPointUsed[i] = true;
                    obj.transform.SetParent(AttachmentPoints[i].transform, false);
                    return true;
                }
            }
        }
        else if (currentPriority == 4 && componentTypeToAdd == ComponentType.bell)
        {
            for (int i = 0; i < attachmentPointUsed.Length; i++)
            {
                if (!attachmentPointUsed[i])
                {
                    attachmentPointUsed[i] = true;
                    obj.transform.SetParent(AttachmentPoints[i].transform, false);
                    return true;
                }
            }
        }

        return false;
    }

    public void Assemble()
    {
        switch (currentPriority)
        {
            case 1:
                bool foundFrame = false;
                bool foundCog = false;

                for (int i = 0; i < AttachmentPoints.Length; i++)
                {
                    if (AttachmentPoints[i].childCount == 1)
                    {
                        ClockComponent cc = AttachmentPoints[i].GetChild(0).GetComponent<ClockComponent>();

                        if (cc.Type == ComponentType.frame && !foundFrame)
                        {
                            foundFrame = true;
                            frame = cc.gameObject;
                        }
                        else if (cc.Type == ComponentType.smallCog || cc.Type == ComponentType.mediumCog || cc.Type == ComponentType.bigCog)
                        {
                            foundCog = true;
                            cogs.Add(cc.gameObject);
                        }
                    }
                }

                if (foundFrame && foundCog)
                {
                    currentPriority++;
                    ResetAttachmentPoints();
                    spawnManager.Spawn();
                }
                break;
            case 2:
                bool foundNumbers = false;
                for (int i = 0; i < AttachmentPoints.Length; i++)
                {
                    ClockComponent cc = AttachmentPoints[i].GetChild(0).GetComponent<ClockComponent>();

                    if (cc.Type == ComponentType.numbers && !foundNumbers)
                    {
                        foundNumbers = true;
                        numbers = cc.gameObject;
                    }
                }

                if (foundNumbers)
                {
                    currentPriority++;
                    ResetAttachmentPoints();
                    spawnManager.Spawn();
                }
                break;
            case 3:
                bool foundSmallHand = false;
                bool foundBigHand = false;
                for (int i = 0; i < AttachmentPoints.Length; i++)
                {
                    ClockComponent cc = AttachmentPoints[i].GetChild(0).GetComponent<ClockComponent>();

                    if (cc.Type == ComponentType.smallHand && !foundSmallHand)
                    {
                        foundSmallHand = true;
                        smallHand = cc.gameObject;
                    }
                    else if (cc.Type == ComponentType.bigHand && !foundBigHand)
                    {
                        foundBigHand = true;
                        bigHand = cc.gameObject;
                    }
                }

                if (foundSmallHand && foundBigHand)
                {
                    currentPriority++;
                    ResetAttachmentPoints();
                    spawnManager.Spawn();
                }
                break;
            case 4:
                bool foundBell = false;
                for (int i = 0; i < AttachmentPoints.Length; i++)
                {
                    ClockComponent cc = AttachmentPoints[i].GetChild(0).GetComponent<ClockComponent>();

                    if (cc.Type == ComponentType.bell && !foundBell)
                    {
                        foundBell = true;
                        bell = cc.gameObject;
                    }
                }

                if (foundBell)
                {
                    currentPriority++;
                    ResetAttachmentPoints();
                    spawnManager.Spawn();
                    // TODO: Check with Challenge Manager
                }
                break;
        }
    }

    public void GiveObjectFromTable(Transform handsOfPlayerTryingToGetItem)
    {
        float minDist = -1.0f;
        float currentDist;
        int minDistIndex = -1;

        for (int i = 0; i < AttachmentPoints.Length; i++)
        {
            if (attachmentPointUsed[i])
            {
                if (Mathf.Approximately(minDist, -1.0f))
                {
                    minDist = Vector3.Distance(handsOfPlayerTryingToGetItem.position, AttachmentPoints[i].position);
                    currentDist = minDist;
                    minDistIndex = i;
                }
                else
                {
                    currentDist = Vector3.Distance(handsOfPlayerTryingToGetItem.position, AttachmentPoints[i].position);

                    if (currentDist < minDist)
                    {
                        minDist = currentDist;
                        minDistIndex = i;
                    }
                }
            }
        }

        if (!Mathf.Approximately(minDist, -1.0f))
        {
            AttachmentPoints[minDistIndex].GetChild(0).SetParent(handsOfPlayerTryingToGetItem, false);
        }
    }

    public bool HasItemsOnTable()
    {
        for (int i = 0; i < attachmentPointUsed.Length; i++)
        {
            if (attachmentPointUsed[i])
            {
                return true;
            }
        }

        return false;
    }

    void ResetAttachmentPoints()
    {
        for (int i = 0; i < AttachmentPoints.Length; i++)
        {
            if (AttachmentPoints[i].childCount == 1)
            {
                Destroy(AttachmentPoints[i].GetChild(0).gameObject);
            }

            attachmentPointUsed[i] = false;
        }
    }

}
