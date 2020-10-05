using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InteractableObject : EnhancedMonoBehaviour
{
    [Range(0, 2)]
    public int InteractionLayer = 0;
    [HideInInspector]
    [SerializeField] protected MaterialsIndexer materialsIndexer = null;
    [HideInInspector]
    [SerializeField] protected GameObject ObjectRef = null;
    [HideInInspector]
    [SerializeField] protected MeshRenderer MeshRef = null;

    private MapBlock Owner = null;

    public virtual void InteractEnter(Character player)
    {
        
    }

    public virtual void InteractExit(Character player)
    {
        
    }

    protected virtual GameObject GetObjectRef()
    {
        return gameObject;
    }

    protected MapBlock GetOwner()
    {
        if (!Owner)
        {
            Owner = gameObject.GetComponent<MapBlock>();
        }

        return Owner;
    }
}
