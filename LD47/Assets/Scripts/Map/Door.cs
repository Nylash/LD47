using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] private MovementCommand WallToConvertToDoor = MovementCommand.None;
    [HideInInspector] [SerializeField] private MovementCommand PreviousDirection = MovementCommand.None;
    [HideInInspector] [SerializeField] private GameObject frameLeftRef = null;
    [HideInInspector] [SerializeField] private GameObject frameRightRef = null;
    
    protected override void EditorStart()
    {
        ObjectRef = GetObjectRef();
        if(ObjectRef)
            MeshRef = ObjectRef.GetComponentInChildren<MeshRenderer>();

        PreviousDirection = WallToConvertToDoor;

        if (MeshRef)
        {
            MeshRef.gameObject.tag = "Door";
            MeshRef.GetComponent<MeshFilter>().mesh = GetOwner().DoorModel;

            SpawnFrames();
            LanternMaterialHandler();
        }
    }

    public override void InteractEnter(Character player)
    {
        GetOwner().UnlockDirection(WallToConvertToDoor);
        ObjectRef.GetComponent<Animator>().SetTrigger("Open");
    }

    public override void InteractExit(Character player)
    {
        GetOwner().LockDirection(WallToConvertToDoor);
        ObjectRef.GetComponent<Animator>().SetTrigger("Close");
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
                MeshRef.sharedMaterial = GetOwner().WallDefaultMaterial;
                MeshRef.GetComponent<MeshFilter>().sharedMesh = GetOwner().WallModel.GetComponentInChildren<MeshFilter>().sharedMesh;
                MeshRef.gameObject.tag = "Untagged";
                Destroy(frameLeftRef);
                Destroy(frameRightRef);
            }
            EditorStart();
        }
        LanternMaterialHandler();
    }

    void SpawnFrames()
    {
        if (!frameLeftRef && !frameRightRef)
        {
            if (WallToConvertToDoor == MovementCommand.Down || WallToConvertToDoor == MovementCommand.Up)
            {
                frameLeftRef = Instantiate(GetOwner().FrameModel, MeshRef.transform.position, Quaternion.identity);
                frameLeftRef.transform.position += new Vector3(-.5f, 0, 0);
                frameRightRef = Instantiate(GetOwner().FrameModel, MeshRef.transform.position, Quaternion.identity);
                frameRightRef.transform.position += new Vector3(.5f, 0, 0);
            }
            if (WallToConvertToDoor == MovementCommand.Left || WallToConvertToDoor == MovementCommand.Right)
            {
                frameLeftRef = Instantiate(GetOwner().FrameModel, MeshRef.transform.position, Quaternion.identity);
                frameLeftRef.transform.position += new Vector3(0, 0, -.5f);
                frameRightRef = Instantiate(GetOwner().FrameModel, MeshRef.transform.position, Quaternion.identity);
                frameRightRef.transform.position += new Vector3(0, 0, .5f);
            }
            frameLeftRef.transform.parent = GetOwner().transform;
            frameRightRef.transform.parent = GetOwner().transform;
        }
    }

    void LanternMaterialHandler()
    {
        if(frameLeftRef && frameRightRef)
        {
            if (frameLeftRef.transform.childCount > 0)
            {
                if (frameLeftRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1] != materialsIndexer.materials[InteractionLayer])
                {
                    Material[] mat = frameLeftRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
                    mat[1] = materialsIndexer.materials[InteractionLayer];
                    frameLeftRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = mat;
                }
            }
            if (frameRightRef.transform.childCount > 0)
            {
                if (frameRightRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[1] != materialsIndexer.materials[InteractionLayer])
                {
                    Material[] mat = frameRightRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
                    mat[1] = materialsIndexer.materials[InteractionLayer];
                    frameRightRef.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = mat;
                }
            }
        }
    }
}
