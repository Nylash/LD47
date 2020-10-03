using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Map : EnhancedMonoBehaviour
{
    [SerializeField] private Vector2 Size = new Vector2(10,10);
    [SerializeField]
    [HideInInspector]
    private Vector2 PreviousSize = new Vector2(-1,-1);
    [SerializeField] private List<MapBlock> Blocks = new List<MapBlock>();

    [SerializeField] private bool RefreshAssets = false; 

    private List<Vector2> ActiveBlockCoords = new List<Vector2>();
    private List<Vector2> NewActiveBlockCoords = new List<Vector2>();
    private List<Vector2> NewInactiveBlockCoords = new List<Vector2>();
    
    private List<Character> Ghosts = new List<Character>();
    
    protected override void EditorUpdate()
    {
        if (RefreshAssets)
        {
            CreateBlocks(true);
            RefreshAssets = false;
        }
        
        if (Size != PreviousSize)
        {
            CreateBlocks();
            PreviousSize = Size;
        }
    }
    
    protected override void GameUpdate()
    {
        for (int i = 0; i < NewInactiveBlockCoords.Count; ++i)
        {
            // If the block wasn't activated again this frame, delete it trigger exit on buttons
            if (!NewActiveBlockCoords.Contains(NewInactiveBlockCoords[i]))
            {
                ActiveBlockCoords.Remove(NewInactiveBlockCoords[i]);
                Button button = GetBlock(NewInactiveBlockCoords[i]).GetComponent<Button>();
                if(button)
                {
                    button.InteractExit();
                }
            }
        }
        for (int i = 0; i < NewActiveBlockCoords.Count; ++i)
        {
            // If the block wasn't activated before, add it and trigger button enter
            if (!ActiveBlockCoords.Contains(NewActiveBlockCoords[i]))
            {
                ActiveBlockCoords.Add(NewActiveBlockCoords[i]);
                Button button = GetBlock(NewActiveBlockCoords[i]).GetComponent<Button>();
                if(button)
                {
                    button.InteractEnter();
                }
            }
        }
        
        NewActiveBlockCoords.Clear();
        NewInactiveBlockCoords.Clear();
    }

    private void CreateBlocks(bool FromExisting = false)
    {
        List<MapBlock> newBlocks = new List<MapBlock>();
        for (int x = 0; x < Size.x; ++x)
        {
            for (int y = 0; y < Size.y; ++y)
            {
                GameObject newBlock = new GameObject("Block ("+x+","+y+")");
                newBlock.transform.parent = transform;

                MapBlock blockComponent = newBlock.AddComponent<MapBlock>();
                blockComponent.SetCoordinates(new Vector2(x,y));

                if (FromExisting)
                {
                    MapBlock block = GetBlock(new Vector2(x, y));
                    blockComponent.Copy(block);
                    blockComponent.UpdateWalls();
                }
                newBlocks.Add(blockComponent);
            }    
        }
        
        for(int i = Blocks.Count - 1; i >= 0; --i)
        {
            DestroyImmediate(Blocks[i].gameObject);
        }
        Blocks.Clear();
        Blocks.AddRange(newBlocks);
    }

    public bool CanMoveTo(Vector2 From, MovementCommand Direction, out Vector2 NewCoordinates)
    {
        NewCoordinates = From;
        if (GetBlock(From).BlockMovement(Direction))
        {
            print("Block by current");
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
            print("Blocked by other");
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
    
    public void CharacterOnBlock(Vector2 Coords)
    {
        NewActiveBlockCoords.Add(Coords);
    }
    
    public void CharacterLeaveBlock(Vector2 Coords)
    {
        NewInactiveBlockCoords.Add(Coords);
    }

    public void AddGhost(Character NewGhost)
    {
        
    }
}
