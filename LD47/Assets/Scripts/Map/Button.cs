using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Button : InteractableObject
{
    [HideInInspector]
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();
    [HideInInspector]
    [SerializeField] private GameObject ButtonModel = null;

    public override void InteractEnter()
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractEnter();
        }
    }

    public override void InteractExit()
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractExit();
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
