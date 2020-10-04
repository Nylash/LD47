using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBarrier : MonoBehaviour
{
    [SerializeField] private List<Mesh> barriers = new List<Mesh>();

    private void Start()
    {
        if(tag != "Door")
            GetComponent<MeshFilter>().mesh = barriers[Random.Range(0, barriers.Count)];
    }
}
