using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterButton : MonoBehaviour
{
    private List<Button> AllButtonsUnsorted = new List<Button>();
    private Button[] AllButtonsSorted;
    private List<InteractableObject> AllInteractableObjects = new List<InteractableObject>();

    private void Start()
    {
        AllButtonsUnsorted.AddRange(FindObjectsOfType<Button>());
        AllInteractableObjects.AddRange(FindObjectsOfType<InteractableObject>());
        AllButtonsSorted = new Button[AllButtonsUnsorted.Count];
        foreach (Button item in AllButtonsUnsorted)
        {
            AllButtonsSorted[item.Index] = item;
        }
        foreach (InteractableObject item in AllInteractableObjects)
        {
            AllButtonsSorted[item.Index].relatedObjects.Add(item);
        }
    }
}
