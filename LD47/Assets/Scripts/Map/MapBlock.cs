using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapBlock : MonoBehaviour
{

    [Header("Block data")] 
    [SerializeField] private Vector2 Coordinates;
    
    [Header("Walls")]
    [SerializeField] private bool bHasWallTop = false;
    [SerializeField] private bool bHasWallLeft = false;
    [SerializeField] private bool bHasWallBottom = false;
    [SerializeField] private bool bHasWallRight = false;
    [SerializeField] private GameObject WallModel = null;

    [Header("Floor")]
    [SerializeField] private GameObject FloorModel = null;
    
    private GameObject[] WallsRef = {null, null, null, null};

    private void Start()
    {
        Instantiate(FloorModel, transform);
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

    private void GameUpdate()
    {
        
    }

    private void EditorUpdate()
    {
       UpdateWalls();
    }

    public void SetCoordinates(Vector2 NewCoordinates)
    {
        Coordinates = NewCoordinates;
        transform.position = transform.parent.position + new Vector3(Coordinates.x,0,-Coordinates.y);
    }

    private void UpdateWalls()
    {
        UpdateWall(bHasWallTop, 0, new Vector3(0,0,0.5f), new Vector3(0,90,0));
        UpdateWall(bHasWallLeft, 1, new Vector3(-0.5f,0,0), Vector3.zero);
        UpdateWall(bHasWallBottom, 2, new Vector3(0,0,-0.5f), new Vector3(0,90,0));
        UpdateWall(bHasWallRight, 3, new Vector3(0.5f,0,0), Vector3.zero);
    }

    private void UpdateWall(bool ShouldHaveWall, int WallIndex, Vector3 Location, Vector3 Rotation)
    {
        if (ShouldHaveWall && !WallsRef[WallIndex])
        {
            WallsRef[WallIndex] = Instantiate(WallModel, transform);
            WallsRef[WallIndex].transform.localPosition = Location;
            WallsRef[WallIndex].transform.localRotation = Quaternion.Euler(Rotation);
        }
        else if (!ShouldHaveWall && WallsRef[WallIndex])
        {
            DestroyImmediate(WallsRef[WallIndex]);
            WallsRef[WallIndex] = null;
        }
    }
}
