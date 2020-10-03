using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private void Start()
    {
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
            }
            
            PreviousCommand.Add(Command);
            Command = MovementCommand.None; 
        }
    }

    public void AskMoveUp()
    {
        if(Command == MovementCommand.None && GetLastCommand() != MovementCommand.Down)
            Command = MovementCommand.Up;
    }
    
    public void AskMoveDown()
    {
        if(Command == MovementCommand.None && GetLastCommand() != MovementCommand.Up)
            Command = MovementCommand.Down;
    }
    
    public void AskMoveLeft()
    {
        if(Command == MovementCommand.None && GetLastCommand() != MovementCommand.Right)
            Command = MovementCommand.Left;   
    }
    
    public void AskMoveRight()
    {
        if(Command == MovementCommand.None && GetLastCommand() != MovementCommand.Left)
            Command = MovementCommand.Right;
    }

    public void CreateNext()
    {
        
    }

    private void Move(Vector2 newCoordinates)
    {
        Coordinates = newCoordinates;
        transform.position = MapReference.MapCoordinatesToWorldSpace(newCoordinates);
    }

    private MovementCommand GetLastCommand()
    {
        return PreviousCommand.Count == 0 ? MovementCommand.None : PreviousCommand.Last();
    }
}
