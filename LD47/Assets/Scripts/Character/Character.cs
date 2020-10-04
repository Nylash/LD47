using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public enum MovementCommand
{
    None,
    Up,
    Left,
    Down,
    Right
}

public class Character : MonoBehaviour
{
    [SerializeField] private Map MapReference = null;

    private MovementCommand Command = MovementCommand.None;

    private List<MovementCommand> PreviousCommand = new List<MovementCommand>();
    private int CurrentCommandIndex = 0;

    [SerializeField] private Vector2 Coordinates = Vector2.zero;
    private Vector2 PreviousCoordinates = Vector2.zero;

    private Vector2 InitialCoordinates = Vector2.zero;

    public bool GhostCreationRequested = false;

    private int NumberOfGhostCreated = 0;
    
    // Movement
    [Header("Movement")]
    [HideInInspector] public bool IsMoving = false;
    private Vector3 PositionMovementStart = Vector3.zero;
    private Vector3 PositionMovementEnd = Vector3.zero;
    [SerializeField] private float MovementSpeed = 0.5f;
    private float TimeElapsedSinceMovementAsked = 0;
    [SerializeField] private AnimationCurve MovementCurve = new AnimationCurve();

    private bool LoopLocked = false;

    public UnityAction OnMovementComplete;

    [Header("FX")] 
    [SerializeField]
    private GameObject GhostTrailPrefab = null;
    [SerializeField]
    private MaterialsIndexer MaterialIndexer = null;

    [SerializeField] private Color Color = Color.white;

    private GhostPath GhostPath = null;

    public void InitializeFromCharacter(Character Other)
    {
        SetInitialCoordinates(Other.InitialCoordinates);
        Coordinates = InitialCoordinates;
        PreviousCommand.AddRange(Other.PreviousCommand);
        MapReference = Other.MapReference;
        transform.position = MapReference.MapCoordinatesToWorldSpace(InitialCoordinates);

        MapReference.CharacterOnBlock(this);
        GhostPath = Other.GhostPath;
        GhostPath.SetColor(Color);
    }
    
    private void Awake()
    {
        if (IsPlayer())
        {
            SetInitialCoordinates(Coordinates);
            MapReference = FindObjectOfType<Map>();
            MapReference.RegisterPlayer(this);
            MapReference.OnGameOver += GameOver;
            GameObject go = Instantiate(GhostTrailPrefab, MapReference.MapCoordinatesToWorldSpace(Coordinates), Quaternion.Euler(0,0,0));
            GhostPath = go.GetComponent<GhostPath>();
            GhostPath.SetInitialPoint(InitialCoordinates);
            GhostPath.SetColor(Color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            TimeElapsedSinceMovementAsked += Time.deltaTime;
            float alpha = Mathf.Clamp01(TimeElapsedSinceMovementAsked / MovementSpeed);
            transform.position = Vector3.Lerp(PositionMovementStart, PositionMovementEnd, MovementCurve.Evaluate(alpha));
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(PositionMovementStart - PositionMovementEnd), alpha);
            if (alpha >= 1)
            {
                IsMoving = false;
                OnMovementComplete.Invoke();
            }
        }
        else
        {
            if (IsPlayer())
            {
                if (Command != MovementCommand.None)
                {
                    Vector2 newCoordinates;
                    if (MapReference.CanMoveTo(Coordinates, Command, out newCoordinates))
                    {
                        MapReference.ManualUpdate();
                    }
                    else
                    {
                        Command = MovementCommand.None;
                    }
                }
            }
        }
            
        
    }

    public void DoUpdate()
    {
        if (Command != MovementCommand.None)
        {
            Vector2 newCoordinates;
            if (MapReference.CanMoveTo(Coordinates, Command, out newCoordinates))
            {
                if ((int)(newCoordinates - Coordinates).magnitude == 1)
                {
                    Move(newCoordinates);
                }
                else
                {
                    Teleport(newCoordinates);
                }
                if (IsPlayer())
                {
                    if (GhostCreationRequested)
                    {
                        CreateNextGhost();
                        GhostCreationRequested = false;
                    }
                    PreviousCommand.Add(Command);
                    GhostPath.AddCommand(Command);
                }
            }
            
            Command = MovementCommand.None;
        }
    }

    public void AskMoveUp(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !UI_Manager.instance.bLevelPaused && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Down && !IsMoving && !LoopLocked)
            Command = MovementCommand.Up;
    }
    
    public void AskMoveDown(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !UI_Manager.instance.bLevelPaused && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Up && !IsMoving && !LoopLocked)
            Command = MovementCommand.Down;
    }
    
    public void AskMoveLeft(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !UI_Manager.instance.bLevelPaused && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Right && !IsMoving && !LoopLocked)
            Command = MovementCommand.Left;   

    }
    
    public void AskMoveRight(InputAction.CallbackContext ctx)
    {
        if(ctx.started && !UI_Manager.instance.bLevelPaused && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Left && !IsMoving && !LoopLocked)
            Command = MovementCommand.Right;
    }

    public void AskCreateNextGhost(InputAction.CallbackContext ctx)
    {
        if (ctx.started && !UI_Manager.instance.bLevelPaused)
        {
            GhostCreationRequested = true;
        }
    }
    
    public void CreateNextGhost()
    {
        if (PreviousCommand.Count >= 2)
        {
            PreviousCommand.RemoveAt(PreviousCommand.Count - 1);
            Character ghost = MapReference.AddGhost(this);
            ghost.SetColor(MaterialIndexer.GhostColors[NumberOfGhostCreated % MaterialIndexer.GhostColors.Length]);
            NumberOfGhostCreated++;
            
            GameObject go = Instantiate(GhostTrailPrefab, MapReference.MapCoordinatesToWorldSpace(Coordinates), Quaternion.Euler(0,0,0));
            GhostPath = go.GetComponent<GhostPath>();
            
            PreviousCommand.Clear();
            InitialCoordinates = PreviousCoordinates;
            GhostPath.SetInitialPoint(InitialCoordinates);
            GhostPath.SetColor(Color);
        }
    }

    private void Move(Vector2 newCoordinates)
    {
        MapReference.CharacterLeaveBlock(this);
        
        PreviousCoordinates = Coordinates;
        Coordinates = newCoordinates;

        TimeElapsedSinceMovementAsked = 0;
        PositionMovementStart = transform.position;
        PositionMovementEnd = MapReference.MapCoordinatesToWorldSpace(newCoordinates);
        IsMoving = true;
        
        MapReference.CharacterOnBlock(this);
    }

    private void Teleport(Vector2 newCoordinates)
    {
        MapReference.CharacterLeaveBlock(this);
        
        PreviousCoordinates = Coordinates;
        Coordinates = newCoordinates;
        transform.position = MapReference.MapCoordinatesToWorldSpace(newCoordinates);

        MapReference.CharacterOnBlock(this);
        
        OnMovementComplete.Invoke();
    }

    private MovementCommand GetLastCommand()
    {
        return PreviousCommand.Count == 0 ? MovementCommand.None : PreviousCommand.Last();
    }

    private void SetInitialCoordinates(Vector2 Coords)
    {
        InitialCoordinates = Coords;
    }

    public void ReadNextOrder()
    {
        if (CurrentCommandIndex == PreviousCommand.Count)
        {
            CurrentCommandIndex = 0;
            Teleport(InitialCoordinates);
        }
        else
        {
            Command = PreviousCommand[CurrentCommandIndex];
            CurrentCommandIndex++;            
        }
        DoUpdate();
    }

    private bool IsPlayer()
    {
        return gameObject.GetComponent<PlayerInput>() != null;
    }

    public Vector2 GetCoordinates()
    {
        return Coordinates;
    }
    
    public Vector2 GetPreviousCoordinates()
    {
        return PreviousCoordinates;
    }

    private void GameOver()
    {
        LoopLocked = true;
    }

    public void AskPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            UI_Manager.instance.Pause();
    }

    public void SetColor(Color color)
    {
        Color = color;
        GhostPath.SetColor(color);
        
        //TODO apply color to mesh
        SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        foreach (Material mat in meshRenderer.materials)
        {
            mat.SetColor("Color_604A5A12",color); 
        }
    }
}
