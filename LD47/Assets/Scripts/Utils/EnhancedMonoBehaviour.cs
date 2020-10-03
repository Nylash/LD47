using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class EnhancedMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            GameAwake();
        }
        else
        {
            EditorAwake();
        }
#else
        GameAwake();
#endif
    }

    protected virtual void GameAwake()
    {
        
    }
    
    protected virtual void EditorAwake()
    {
        
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            GameStart();
        }
        else
        {
            EditorStart();
        }
#else
        GameStart();
#endif
    }
    
    protected virtual void GameStart()
    {
        
    }
    
    protected virtual void EditorStart()
    {
        
    }

    private void Update()
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
    
    protected virtual void GameUpdate()
    {
        
    }
    
    protected virtual void EditorUpdate()
    {
        
    }
}
