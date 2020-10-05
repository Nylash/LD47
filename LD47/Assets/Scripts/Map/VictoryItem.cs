using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryItem : ButtonGameplay
{
    [HideInInspector]
    [SerializeField] private GameObject VictoryItemModel = null;

    public override void InteractEnter(Character player)
    {
        UI_Manager.instance.AskItemVictory();
        Destroy(ObjectRef);
        Destroy(this);
    }

    public override void InteractExit(Character player)
    {
        return;
    }

    protected override void GameStart()
    {
        GetObjectRef().GetComponent<Animator>().speed = Random.Range(.6f, .8f);
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
            return Instantiate(VictoryItemModel, transform);
        }
        else
        {
            return ObjectRef;
        }
    }
}
