using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryButton : ButtonGameplay
{
    [HideInInspector]
    [SerializeField] private GameObject VictoryModel = null;

    public override void InteractEnter(Character player)
    {
        UI_Manager.instance.Victory();
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
            return Instantiate(VictoryModel, transform);
        }
        else
        {
            return ObjectRef;
        }
    }
}
