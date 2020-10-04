using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Map : EnhancedMonoBehaviour
{
    [SerializeField] private Vector2 Size = new Vector2(10, 10);
    [SerializeField] [HideInInspector] private Vector2 PreviousSize = new Vector2(-1, -1);
    [SerializeField] private List<MapBlock> Blocks = new List<MapBlock>();

    [SerializeField] private bool RefreshAssets = false;

    private List<Vector2> ActiveBlockCoords = new List<Vector2>();
    private List<Character> NewActiveBlockCoords = new List<Character>();
    private List<Character> NewInactiveBlockCoords = new List<Character>();

    [SerializeField] private GameObject GhostPrefab = null;
    private List<Character> Ghosts = new List<Character>();

    public UnityAction OnGameOver;

    private Character PlayerCharacter = null;

    private bool IsUpdating = false;
    private bool IsUpdatingGhost = false;

    private bool IsRewinding = false;

    public void StartRewind()
    {
        IsRewinding = true;
        PlayerCharacter.IsRewinding = true;
        foreach (var ghost in Ghosts)
        {
            ghost.IsRewinding = true;
        }
    }

    public void StopRewind()
    {
        IsRewinding = false;
        PlayerCharacter.IsRewinding = false;
        foreach (var ghost in Ghosts)
        {
            ghost.IsRewinding = false;
        }
    }

    protected override void GameStart()
    {
        OnGameOver += UI_Manager.instance.Pause;
    }
    
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
    
    public void ManualUpdate()
    {
        if (IsUpdating)
        {
            return;
        }

        IsUpdating = true;
        
        // First update ghosts
        PlayerCharacter.DoUpdate();
        UpdateGhosts();
    }

    public void RewindUpdate()
    {
        if (IsUpdating)
        {
            return;
        }
        
        IsUpdating = true;
        
        RewindGhosts();
        PlayerCharacter.ReadPreviousOrder();
    }

    private void UpdateMap()
    {
        for (int i = 0; i < NewInactiveBlockCoords.Count; ++i)
        {
            // If the block wasn't activated again this frame, delete it trigger exit on buttons
            bool found = false;
            foreach (Character character in NewActiveBlockCoords)
            {
                if (character.GetCoordinates() == NewInactiveBlockCoords[i].GetPreviousCoordinates())
                {
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                ActiveBlockCoords.Remove(NewInactiveBlockCoords[i].GetPreviousCoordinates());
                ButtonGameplay button = GetBlock(NewInactiveBlockCoords[i].GetPreviousCoordinates()).GetComponent<ButtonGameplay>();
                if(button)
                {
                    button.InteractExit(NewInactiveBlockCoords[i]);
                }
            }
        }
        for (int i = 0; i < NewActiveBlockCoords.Count; ++i)
        {
            // If the block wasn't activated before, add it and trigger button enter
            if (!ActiveBlockCoords.Contains(NewActiveBlockCoords[i].GetCoordinates()))
            {
                ActiveBlockCoords.Add(NewActiveBlockCoords[i].GetCoordinates());
                ButtonGameplay button = GetBlock(NewActiveBlockCoords[i].GetCoordinates()).GetComponent<ButtonGameplay>();
                if(button)
                {
                    button.InteractEnter(NewActiveBlockCoords[i]);
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

    public bool HasGhostOnTheSameBlock()
    {
        foreach (Character ghost in Ghosts)
        {
            foreach (Character otherGhost in Ghosts)
            {
                if (ghost!= otherGhost && ghost.GetCoordinates() == otherGhost.GetCoordinates())
                {
                    return true;
                }
            }

            if (ghost.GetCoordinates() == PlayerCharacter.GetCoordinates())
            {
                return true;
            }
        }

        return false;
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
    
    public void CharacterOnBlock(Character _character)
    {
        NewActiveBlockCoords.Add(_character);
    }
    
    public void CharacterLeaveBlock(Character _character)
    {
        NewInactiveBlockCoords.Add(_character);
    }

    public Character AddGhost(Character NewGhost)
    {
        GameObject go = Instantiate(GhostPrefab);
        Character ghost = go.GetComponent<Character>();
        ghost.InitializeFromCharacter(NewGhost);
        ghost.OnMovementComplete += OnGhostMovementEnded;
        Ghosts.Add(ghost);
        return ghost;
    }

    public void UpdateGhosts()
    {
        IsUpdatingGhost = true;
        int numberOfGhostAtStart = Ghosts.Count;
        for (int i = 0; i < numberOfGhostAtStart; ++i)
        {
            Ghosts[i].ReadNextOrder();
        }
        IsUpdatingGhost = false;
        
        if (HasGhostOnTheSameBlock())
        {
            OnGameOver.Invoke();
        }
        else
        {
            OnGhostMovementEnded();
        }
    }

    public void RewindGhosts()
    {
        IsUpdatingGhost = true;
        int numberOfGhostAtStart = Ghosts.Count;
        for (int i = 0; i < numberOfGhostAtStart; ++i)
        {
            Ghosts[i].ReadPreviousOrder();
        }
        IsUpdatingGhost = false;
        
        OnGhostMovementEnded();
    }

    public void RegisterPlayer(Character Player)
    {
        PlayerCharacter = Player;
        PlayerCharacter.OnMovementComplete += OnPlayerMovementEnded;
    }

    void OnPlayerMovementEnded()
    {
        // Then update map
        if (!GhostAreMoving() && !IsUpdatingGhost)
        {
            // Check ghost collision
            if (!PlayerCharacter.IsRewinding && HasGhostOnTheSameBlock())
            {
                OnGameOver.Invoke();
            }
            else
            {
                UpdateMap();
            }
        }
        
        IsUpdating = false;
    }

    private bool GhostAreMoving()
    {
        foreach (var ghost in Ghosts)
        {
            if (ghost.IsMoving)
            {
                return true;
            }
        }

        return false;
    }
    
    void OnGhostMovementEnded()
    {
        if (!GhostAreMoving() && !IsUpdatingGhost && !PlayerCharacter.IsMoving)
        {
            // Check ghost collision
            if (!PlayerCharacter.IsRewinding && HasGhostOnTheSameBlock())
            {
                OnGameOver.Invoke();
            }
            else
            {
                UpdateMap();
            }
        }
    }

    public void NotifyGhostDestruction(Character Ghost)
    {
        Ghosts.Remove(Ghost);
        PlayerCharacter.InitializeFromCharacter(Ghost, true);
    }
    
}
