using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPlate : Button
{
    [HideInInspector]
    [SerializeField] private GameObject LoopPlateModel = null;

    public override void InteractEnter(Character player)
    {
        FindObjectOfType<Character>().GhostCreationRequested = true;
    }

    public override void InteractExit(Character player)
    {
        return;
    }

    protected override void EditorStart()
    {
        ObjectRef = GetObjectRef();
    }

    protected override void EditorUpdate()
    {
        return;
    }

    protected override GameObject GetObjectRef()
    {
        if (!ObjectRef)
        {
            return Instantiate(LoopPlateModel, transform);
        }
        else
        {
            return ObjectRef;
        }
    }
}
