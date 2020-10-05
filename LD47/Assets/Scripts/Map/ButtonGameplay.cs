using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ButtonGameplay : InteractableObject
{
    [HideInInspector]
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();

    protected override void EditorStart()
    {
        ObjectRef = GetObjectRef();
        if (ObjectRef)
            MeshRef = ObjectRef.GetComponentInChildren<MeshRenderer>();
        if(MeshRef)
            LanternMaterialHandler();
    }

    protected override void EditorUpdate()
    {
        if (MeshRef)
            LanternMaterialHandler();
    }

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
            return Instantiate(GetOwner().ButtonModel, transform);
        }
        else
        {
            return ObjectRef;
        }
    }

    void LanternMaterialHandler()
    {
        if(MeshRef.transform.childCount > 0)
        {
            if (MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1] != materialsIndexer.materials[InteractionLayer])
            {
                Material[] mat = MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
                mat[1] = materialsIndexer.materials[InteractionLayer];
                MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = mat;
            }
        }
    }
}
