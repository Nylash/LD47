using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullWallPolisher : EnhancedMonoBehaviour
{
    [SerializeField] private bool DisplayWallBot = false;
    [SerializeField] private bool DisplayWallTop = false;
    [SerializeField] private bool DisplayWallLeft = false;
    [SerializeField] private bool DisplayWallRight = false;

    [SerializeField] private GameObject[] WallsRef = new GameObject[4];


    protected override void GameStart()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

    protected override void EditorStart()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }

    protected override void EditorUpdate()
    {
        UpdateWall(DisplayWallBot, 0, new Vector3(0, 0, -0.5f), new Vector3(0, 90, 0));
        UpdateWall(DisplayWallTop, 1, new Vector3(0, 0, 0.5f), new Vector3(0, 90, 0));
        UpdateWall(DisplayWallLeft, 2, new Vector3(-0.5f, 0, 0), Vector3.zero);
        UpdateWall(DisplayWallRight, 3, new Vector3(0.5f, 0, 0), Vector3.zero);
    }

    private void UpdateWall(bool ShouldHaveWall, int indexWall, Vector3 Location, Vector3 Rotation)
    {
        if (ShouldHaveWall && !WallsRef[indexWall])
        {
            WallsRef[indexWall] = Instantiate(GetComponentInParent<MapBlock>().WallModel, transform.parent);
            WallsRef[indexWall].transform.localPosition = Location;
            WallsRef[indexWall].transform.localRotation = Quaternion.Euler(Rotation);
        }
        else if (!ShouldHaveWall && WallsRef[indexWall])
        {
            DestroyImmediate(WallsRef[indexWall]);
            WallsRef[indexWall] = null;
        }
    }
}
