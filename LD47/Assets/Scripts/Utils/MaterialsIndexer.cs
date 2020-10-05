using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Materials Indexer", menuName ="Material Indexer")]
public class MaterialsIndexer : ScriptableObject
{
    public Material[] materials;

    [ColorUsage(true,true)]
    public Color[] GhostColors;

    public Vector4[] materialsColorsDefault;

    public Vector4[] materialsColorsActive;
}
