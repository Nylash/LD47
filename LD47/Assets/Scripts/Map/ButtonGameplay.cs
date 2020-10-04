using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ButtonGameplay : InteractableObject
{
    [HideInInspector]
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();
    [HideInInspector]
    [SerializeField] private GameObject ButtonModel = null;

    public override void InteractEnter(Character player)
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractEnter(player);
        }
    }

    public override void InteractExit(Character player)
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractExit(player);
        }
    }

    protected override GameObject GetObjectRef()
    {
        if (!ObjectRef)
        {
            return Instantiate(ButtonModel, transform);
        }
        else
        {
            return ObjectRef;
        }
    }
}
