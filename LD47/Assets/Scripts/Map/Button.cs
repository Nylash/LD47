using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Button : MonoBehaviour
{
    public int Index = 0;

    [HideInInspector]
    public List<InteractableObject> relatedObjects = new List<InteractableObject>();
    [HideInInspector]
    [SerializeField] private GameObject ButtonModel = null;
    [HideInInspector]
    [SerializeField] private MaterialsIndexer materialsIndexer = null;
    [HideInInspector]
    [SerializeField] private GameObject ButtonRef = null;
    [HideInInspector]
    [SerializeField] private MeshRenderer ButtonMeshRef = null;

    private void Start()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            if (!ButtonRef)
            {
                ButtonRef = Instantiate(ButtonModel, transform);
                ButtonMeshRef = ButtonRef.GetComponent<MeshRenderer>();
            }
        }
#endif
    }

    void Update()
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
        if (ButtonMeshRef.sharedMaterial != materialsIndexer.materials[Index])
            ButtonMeshRef.sharedMaterial = materialsIndexer.materials[Index];
    }

    public void OnButtonEnter()
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractEnter();
        }
    }

    public void OnButtonExit()
    {
        foreach (InteractableObject item in relatedObjects)
        {
            item.InteractExit();
        }
    }
}
