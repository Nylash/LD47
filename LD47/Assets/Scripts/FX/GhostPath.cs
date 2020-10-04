using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostPath : MonoBehaviour
{
    [SerializeField] private float MovingSpeed = 0.5f;
    [SerializeField] private float SpawnRate = 2.0f;
    [SerializeField] private float OutOfBoundOffset = 0.75f;
    [SerializeField] private float Height = 0.1f;
    [SerializeField] private Color Color = Color.white;
    
    private float TimeElapsedSinceLastSpawn = 0;
    private List<float> TimeElapsed = new List<float>();
    private Vector2 InitialPoint;
    private List<Vector2> CurrentPoint = new List<Vector2>();
    private List<MovementCommand> Commands = new List<MovementCommand>();
    private List<int> CurrentIndex = new List<int>();
    
    private List<Vector3> PositionMovementStart = new List<Vector3>();
    private List<Vector3> PositionMovementEnd = new List<Vector3>();

    private Map MapReference = null;
    private GameObject TrailModel = null;

    private List<TrailRenderer> TrailSpawned = new List<TrailRenderer>();
    private List<bool> WaitingDeath = new List<bool>();
    
    
    private void Awake()
    {
        MapReference = FindObjectOfType<Map>();
        TrailModel = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        AddTrail();
    }

    private void Update()
    {
        if (Commands.Count == 0)
            return;

        // remove dead trails
        for (int i = TrailSpawned.Count - 1; i >= 0; --i)
        {
            if (TrailSpawned[i] == null)
            {
                TrailSpawned.RemoveAt(i);
                CurrentPoint.RemoveAt(i);
                TimeElapsed.RemoveAt(i);
                CurrentIndex.RemoveAt(i);
                PositionMovementStart.RemoveAt(i);
                PositionMovementEnd.RemoveAt(i);
                WaitingDeath.RemoveAt(i);
            }
        }
        
        
        // Update trails
        for (int i = 0; i < TrailSpawned.Count; ++i)
        {
            TimeElapsed[i] += Time.deltaTime;
            float alpha = Mathf.Clamp01(TimeElapsed[i] / MovingSpeed);
            print(alpha);
            TrailSpawned[i].gameObject.transform.position = Vector3.Lerp(PositionMovementStart[i], PositionMovementEnd[i], alpha) + Vector3.up * Height;
            if (alpha >= 1 && !WaitingDeath[i])
            {
                AskMoveNextPoint(i);
            }
            
        }
        
        // Spawn new trail x times
        TimeElapsedSinceLastSpawn += Time.deltaTime;
        if (TimeElapsedSinceLastSpawn >= SpawnRate)
        {
            AddTrail();
            TimeElapsedSinceLastSpawn = 0;
        }
    }

    private void AskMoveNextPoint(int index)
    {
        TimeElapsed[index] -= MovingSpeed;
        
        if (CurrentIndex[index] >= Commands.Count - 1)
        {
            CurrentIndex[index] = Commands.Count - 1;
            WaitingDeath[index] = true;
            return;
        }
        else
        {
            CurrentIndex[index]++;
        }
        
        MovementCommand command = Commands[CurrentIndex[index]];

        Vector3 offset = Vector3.up * Height;
        
        Vector2 newCoord;
        PositionMovementStart[index] = MapReference.MapCoordinatesToWorldSpace(CurrentPoint[index]) + offset;
        if (MapReference.CanMoveTo(CurrentPoint[index], command, out newCoord))
        {
            if ((int) (CurrentPoint[index] - newCoord).magnitude != 1)
            {
                WaitingDeath[index] = true;
                AddTrailFromExisting(index, newCoord);
                switch (Commands[CurrentIndex[index]])
                {
                    case MovementCommand.Up :
                        offset.z = OutOfBoundOffset;
                        break;
            
                    case MovementCommand.Down :
                        offset.z = -OutOfBoundOffset;
                        break;
            
                    case MovementCommand.Left :
                        offset.x = -OutOfBoundOffset;
                        break;
            
                    case MovementCommand.Right :
                        offset.x = OutOfBoundOffset;
                        break;
                }
                
                PositionMovementEnd[index] = PositionMovementStart[index] + offset;
            }
            else
            {
                CurrentPoint[index] = newCoord;
                PositionMovementEnd[index] = MapReference.MapCoordinatesToWorldSpace(CurrentPoint[index]) + offset;
            }
        }
        
    }
    
    public void SetInitialPoint(Vector2 Point)
    {
        InitialPoint = Point;
    }
    
    public void AddCommand(MovementCommand Command)
    {
        Commands.Add(Command);
    }

    private void AddTrail()
    {
        GameObject go = Instantiate(TrailModel,
            MapReference.MapCoordinatesToWorldSpace(InitialPoint) + Vector3.up * Height, Quaternion.Euler(0, 0, 0));
        TrailSpawned.Add(go.GetComponent<TrailRenderer>());
        TrailSpawned.Last().material.color = Color;
        TimeElapsed.Add(0);
        PositionMovementStart.Add(go.transform.position);
        PositionMovementEnd.Add(go.transform.position);
        CurrentPoint.Add(InitialPoint);
        WaitingDeath.Add(false);
        CurrentIndex.Add(-1);
    }

    private void AddTrailFromExisting(int Index, Vector2 NextCoord)
    {
        Vector3 offset = Vector3.up * Height;
        switch (Commands[CurrentIndex[Index]])
        {
            case MovementCommand.Up :
                offset.z = -OutOfBoundOffset;
                break;
            
            case MovementCommand.Down :
                offset.z = OutOfBoundOffset;
                break;
            
            case MovementCommand.Left :
                offset.x = OutOfBoundOffset;
                break;
            
            case MovementCommand.Right :
                offset.x = -OutOfBoundOffset;
                break;
        }
        
        GameObject go = Instantiate(TrailModel,MapReference.MapCoordinatesToWorldSpace(NextCoord) + offset,
            Quaternion.Euler(0, 0, 0));
        TrailSpawned.Add(go.GetComponent<TrailRenderer>());
        TrailSpawned.Last().material.color = Color;
        TimeElapsed.Add(0);
        PositionMovementStart.Add(go.transform.position);
        PositionMovementEnd.Add(go.transform.position);
        CurrentPoint.Add(NextCoord);
        WaitingDeath.Add(false);
        CurrentIndex.Add(CurrentIndex[Index]);
    }

    public void SetColor(Color color)
    {
        Color = color;
        foreach (var Trail in TrailSpawned)
        {
            Trail.material.color = Color;
        }
    }
}
