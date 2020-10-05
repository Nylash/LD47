using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPlate : ButtonGameplay
{
    [HideInInspector]
    [SerializeField] private GameObject LoopPlateModel = null;

    public override void InteractEnter(Character player)
    {
        player.GhostCreationRequested = true;
    }

    protected override void GameStart()
    {
        
    }

    public override void InteractExit(Character player)
    {
        return;
    }

    protected override void EditorStart()
    {
        ObjectRef = GetObjectRef();
        InteractionLayer = -1;
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
