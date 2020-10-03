using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    [SerializeField] private Vector2 Size = new Vector2(10,10);
    [SerializeField]
    [HideInInspector]
    private Vector2 PreviousSize = new Vector2(-1,-1);
    [SerializeField] private List<MapBlock> Blocks = new List<MapBlock>();

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

    public bool CanMoveTo(Vector2 From, MovementCommand Direction, out Vector2 NewCoordinates)
    {
        NewCoordinates = From;
        if (GetBlock(From).BlockMovement(Direction))
        { 
            return false;
        }
        
        switch (Direction)
        {
            case MovementCommand.Up:
                NewCoordinates.y -= 1;
                break;
            case MovementCommand.Down:
                NewCoordinates.y += 1;
                break;
            case MovementCommand.Left:
                NewCoordinates.x -= 1;
                break;
            case MovementCommand.Right:
                NewCoordinates.x += 1;
                break;
        }

        ClampCoordinates(ref NewCoordinates);

        if (GetBlock(NewCoordinates).BlockMovement(Direction, false))
        {
            return false;
        }

        return true;
    }

    public Vector3 MapCoordinatesToWorldSpace(Vector2 Coordinates)
    {
        return transform.position + new Vector3(Coordinates.x, 0, -Coordinates.y);
    }

    private MapBlock GetBlock(Vector2 Coordinates)
    {
        return Blocks[(int)Coordinates.x * (int)Size.y + (int)Coordinates.y];
    }

    public void ClampCoordinates(ref Vector2 Coordinates)
    {
        Coordinates.x = Coordinates.x % Size.x;
        Coordinates.y = Coordinates.y % Size.y;

        if (Coordinates.x < 0)
        {
            Coordinates.x += Size.x;
        }

        if (Coordinates.y < 0)
        {
            Coordinates.y += Size.y;
        }
    }
}
