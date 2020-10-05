using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBarrier : EnhancedMonoBehaviour
{
    [SerializeField] private List<Mesh> barriers = new List<Mesh>();

    protected override void GameStart()
    {
        if(tag != "Door")
            GetComponent<MeshFilter>().mesh = barriers[Random.Range(0, barriers.Count)];
    }

    protected override void EditorStart()
    {
        if (tag != "Door")
            GetComponent<MeshFilter>().mesh = barriers[Random.Range(0, barriers.Count)];
    }
}
