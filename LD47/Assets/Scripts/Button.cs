using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();

    public void OnStepOn()
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.Interact();
        }
    }
}
