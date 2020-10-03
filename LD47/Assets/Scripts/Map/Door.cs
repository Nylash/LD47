using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] private MovementCommand WallToConvertToDoor = MovementCommand.None;
    [HideInInspector] [SerializeField] private MovementCommand PreviousDirection = MovementCommand.None;
    [SerializeField] private Material WallDefaultMaterial = null;
    
    protected override void EditorStart()
    {
        
        base.EditorStart();
        PreviousDirection = WallToConvertToDoor;
    }

    public override void InteractEnter()
    {
        GetOwner().UnlockDirection(WallToConvertToDoor);
        ObjectRef.transform.position -= new Vector3(0, .5f, 0);
    }

    public override void InteractExit()
    {
        GetOwner().LockDirection(WallToConvertToDoor);
        ObjectRef.transform.position += new Vector3(0, .5f, 0);
    }

    protected override GameObject GetObjectRef()
    {
        return GetOwner().GetWall(WallToConvertToDoor);
    }

    protected override void EditorUpdate()
    {
        if (WallToConvertToDoor != PreviousDirection)
        {
            if (MeshRef)
            {
                MeshRef.sharedMaterial = WallDefaultMaterial;
            }
            EditorStart();
        }
        base.EditorUpdate();
    }
}
