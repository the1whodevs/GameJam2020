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
    public void AddComponentToTable(GameObject obj)
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
                    break;
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
                    break;
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
                    break;
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
                    break;
                }
            }
        }
    }

    public void Assemble()
    {
        // Get all CHILD gameobjects from all attachment points (AttachmentPoints either have 0 childCount, or 1, ALWAYS)
        // Get the ClockComponent component for all the child gameobjects.

        // For each priority, set the respective variable (e.g. currentPriority == 1 then set frame variable 
        // to the frame game object (check using ClockComponent.Type)

        // Make sure, for each priority, that we have the minimum required parts on the table
        // If we do, .SetParent all of the parts to the AssemblyPoint, and 
        // set the entire attachmentPointUsed array to false (so we can attach objects again!)

        // If the assemble is completed, increase currentPriority by 1!

        // If currentPriority == 5, ClockReady = true!

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
                }
                break;
        }
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
