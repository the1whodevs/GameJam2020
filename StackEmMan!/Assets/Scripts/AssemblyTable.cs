using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyTable : MonoBehaviour
{
    public static AssemblyTable instance;

    [SerializeField] private Transform AssemblyPoint;

    private List<GameObject> DisassembledObjOnTable = new List<GameObject>();
    private List<GameObject> AssembledObjOnTable = new List<GameObject>();

    [SerializeField] private Transform[] AttachmentPoints;

    private bool[] attachmentPointUsed;

    private Clock clock;

    private GameObject smallHand, bigHand, numbers, frame, bell;
    //cogs

    private int currentPriority, smallHandPriority, bigHandPriority, numbersPriority, framePriority, bellPriority, cogsPriority;

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

    public void AssembleDisassembledObjOnTable()
    {
        switch (currentPriority)
        {
            case 1: 
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

}
