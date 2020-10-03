using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    [SerializeField] private Vector2 Size = new Vector2(10,10);
    private Vector2 PreviousSize;
    [SerializeField] private List<MapBlock> Blocks = new List<MapBlock>();

    private void Awake()
    {
        PreviousSize = Size;
        CreateBlocks();
    }

    // Update is called once per frame
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

    private void EditorUpdate()
    {
        if (Size != PreviousSize)
        {
            for(int i = Blocks.Count - 1; i >= 0; --i)
            {
                DestroyImmediate(Blocks[i].gameObject);                
            }
            
            Blocks.Clear();
            CreateBlocks();
            PreviousSize = Size;
        }
    }
    
    private void GameUpdate()
    {
        
    }

    private void CreateBlocks()
    {
        for (int x = 0; x < Size.x; ++x)
        {
            for (int y = 0; y < Size.y; ++y)
            {
                GameObject newBlock = new GameObject("Block ("+x+","+y+")");
                newBlock.transform.parent = transform;

                MapBlock blockComponent = newBlock.AddComponent<MapBlock>();
                blockComponent.SetCoordinates(new Vector2(x,y));
                Blocks.Add(blockComponent);                
            }    
        }
    }
}
