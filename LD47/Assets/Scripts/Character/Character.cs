using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    
    [SerializeField]
    private Vector2 Coordinates = Vector2.zero;

    private Vector2 InitialCoordinates = Vector2.zero;

    public void InitializeFromCharacter(Character Other)
    {
        SetInitialCoordinates(Other.InitialCoordinates);
        Coordinates = InitialCoordinates;
        PreviousCommand.AddRange(Other.PreviousCommand);
        MapReference = Other.MapReference;
        transform.position = MapReference.MapCoordinatesToWorldSpace(InitialCoordinates);

        MapReference.CharacterOnBlock(Coordinates);
    }
    
    private void Start()
    {
        if (IsPlayer())
        {
            SetInitialCoordinates(Coordinates);    
        }
        MapReference = FindObjectOfType<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Command != MovementCommand.None)
        {
            Vector2 newCoordinates;
            if (MapReference.CanMoveTo(Coordinates, Command, out newCoordinates))
            {
                Move(newCoordinates);
                if (IsPlayer())
                {
                    PreviousCommand.Add(Command);
                    MapReference.UpdateGhosts();
                }
            }
            
            Command = MovementCommand.None;
        }
    }

    public void AskMoveUp(InputAction.CallbackContext ctx)
    {
        if(ctx.started && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Down)
            Command = MovementCommand.Up;
    }
    
    public void AskMoveDown(InputAction.CallbackContext ctx)
    {
        if(ctx.started && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Up)
            Command = MovementCommand.Down;
    }
    
    public void AskMoveLeft(InputAction.CallbackContext ctx)
    {
        if(ctx.started && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Right)
            Command = MovementCommand.Left;   

    }
    
    public void AskMoveRight(InputAction.CallbackContext ctx)
    {
        if(ctx.started && Command == MovementCommand.None && GetLastCommand() != MovementCommand.Left)
            Command = MovementCommand.Right;
    }

    public void AskCreateNextGhost(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            CreateNextGhost();
        }
    }
    
    public void CreateNextGhost()
    {
        if (PreviousCommand.Count >= 2)
        {
            PreviousCommand.RemoveAt(PreviousCommand.Count - 1);
            MapReference.AddGhost(this);
            
            PreviousCommand.Clear();
            InitialCoordinates = Coordinates;
        }
    }

    private void Move(Vector2 newCoordinates)
    {
        MapReference.CharacterLeaveBlock(Coordinates);
        
        Coordinates = newCoordinates;
        transform.position = MapReference.MapCoordinatesToWorldSpace(newCoordinates);

        MapReference.CharacterOnBlock(Coordinates);
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
            Move(InitialCoordinates);
        }
        else
        {
            Command = PreviousCommand[CurrentCommandIndex];
            CurrentCommandIndex++;            
        }
        
    }

    private bool IsPlayer()
    {
        return gameObject.GetComponent<PlayerInput>() != null;
    }
}
