using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterButton : MonoBehaviour
{
    private List<Button> AllButtonsUnsorted = new List<Button>();
    private Dictionary<int, List<Button>> AllButtonsSorted;
    private List<InteractableObject> AllInteractableObjects = new List<InteractableObject>();

    private void Start()
    {
        AllButtonsUnsorted.AddRange(FindObjectsOfType<Button>());
        AllInteractableObjects.AddRange(FindObjectsOfType<InteractableObject>());
        AllButtonsSorted = new Dictionary<int, List<Button>>();
        foreach (Button item in AllButtonsUnsorted)
        {
            if (!AllButtonsSorted.ContainsKey(item.InteractionLayer))
            {
                AllButtonsSorted.Add(item.InteractionLayer, new List<Button>());
            }
            AllButtonsSorted[item.InteractionLayer].Add(item);
        }
        foreach (InteractableObject item in AllInteractableObjects)
        {
            if (item.GetType() == typeof(Button) || item.GetType() == typeof(LoopPlate))
            {
                continue;
            }
            
            foreach (Button button in AllButtonsSorted[item.InteractionLayer])
            {
                button.relatedObjects.Add(item);
            }
        }
    }
}
