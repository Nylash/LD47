using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public abstract class InteractableObject : MonoBehaviour
{
    public int Index = 0;
    [HideInInspector]
    [SerializeField] protected MaterialsIndexer materialsIndexer = null;
    [HideInInspector]
    [SerializeField] protected GameObject ObjectRef = null;
    [HideInInspector]
    [SerializeField] protected MeshRenderer MeshRef = null;

    public abstract void InteractEnter();
    public abstract void InteractExit();

    protected virtual void Start()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            if (!ObjectRef)
            {
                ObjectRef = gameObject;
                MeshRef = ObjectRef.GetComponent<MeshRenderer>();
            }
        }
#endif  
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            GameUpdate();
        }
        else
        {
            EditorUpdate();
        }
#else
        GameUpdate();
#endif
    }

    private void GameUpdate()
    {

    }

    private void EditorUpdate()
    {
        if (MeshRef.sharedMaterial != materialsIndexer.materials[Index])
            MeshRef.sharedMaterial = materialsIndexer.materials[Index];
    }
}
