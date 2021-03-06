﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ButtonGameplay : InteractableObject
{
    [HideInInspector]
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();

    private AudioSource audioSource;

    protected override void EditorStart()
    {
        ObjectRef = GetObjectRef();
        if (ObjectRef)
            MeshRef = ObjectRef.GetComponentInChildren<MeshRenderer>();
        if(MeshRef)
            LanternMaterialHandler();
        MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", materialsIndexer.materialsColorsDefault[InteractionLayer]);
    }

    protected override void EditorUpdate()
    {
        if (MeshRef)
            LanternMaterialHandler();
    }

    protected override void GameStart()
    {
        audioSource = ObjectRef.GetComponent<AudioSource>();
        MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", materialsIndexer.materialsColorsDefault[InteractionLayer]);
    }

    public override void InteractEnter(Character player)
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundName.door, audioSource);
        MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", materialsIndexer.materialsColorsActive[InteractionLayer]);
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractEnter(player);
        }
    }

    public override void InteractExit(Character player)
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundName.door, audioSource);
        MeshRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1].SetColor("_EmissionColor", materialsIndexer.materialsColorsDefault[InteractionLayer]);
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
