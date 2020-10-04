using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterButton : MonoBehaviour
{
    private List<ButtonGameplay> AllButtonsUnsorted = new List<ButtonGameplay>();
    private Dictionary<int, List<ButtonGameplay>> AllButtonsSorted;
    private List<InteractableObject> AllInteractableObjects = new List<InteractableObject>();

    private void Start()
    {
        AllButtonsUnsorted.AddRange(FindObjectsOfType<ButtonGameplay>());
        AllInteractableObjects.AddRange(FindObjectsOfType<InteractableObject>());
        AllButtonsSorted = new Dictionary<int, List<ButtonGameplay>>();
        foreach (ButtonGameplay item in AllButtonsUnsorted)
        {
            if (!AllButtonsSorted.ContainsKey(item.InteractionLayer))
            {
                AllButtonsSorted.Add(item.InteractionLayer, new List<ButtonGameplay>());
            }
            AllButtonsSorted[item.InteractionLayer].Add(item);
        }
        foreach (InteractableObject item in AllInteractableObjects)
        {
            if (item.GetType() == typeof(ButtonGameplay))
            {
                continue;
            }
            
            foreach (ButtonGameplay button in AllButtonsSorted[item.InteractionLayer])
            {
                button.relatedObjects.Add(item);
            }
        }
    }
}
