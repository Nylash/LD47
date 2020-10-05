using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsRandomizer : EnhancedMonoBehaviour
{
    [SerializeField] private GameObject[] grassAssets = null;
    [SerializeField] private GameObject[] leafAssets = null;
    [SerializeField] private GameObject[] packLeafsAssets = null;
    [SerializeField] private GameObject[] stoneAssets = null;
    [HideInInspector]
    [SerializeField] private List<GameObject> assetsRef = new List<GameObject>();

    protected override void EditorStart()
    {
        foreach (GameObject item in assetsRef)
            DestroyImmediate(item);
        if (!GetComponentInParent<VictoryButton>() && !GetComponentInParent<LoopPlate>())
        {
            for (float x = -.4f; x < .45f; x += .2f)
            {
                for (float z = -.4f; z < .45f; z += .2f)
                {
                    SpawnProps(x, z);
                }
            }
        }
        else
        {
            SpawnProps(-.4f, -.4f);
            SpawnProps(.4f, -.4f);
            SpawnProps(-.4f, .4f);
            SpawnProps(.4f, .4f);
            SpawnProps(.4f, 0);
            SpawnProps(0, .4f);
            SpawnProps(-.4f, 0);
            SpawnProps(0, -.4f);
        }
    }

    void SpawnProps(float x, float z)
    {
        PropsType randomType = GetRandomType();
        switch (randomType)
        {
            case PropsType.grass:
                GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                grass.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                assetsRef.Add(grass);
                break;
            case PropsType.leaf:
                GameObject leaf = Instantiate(packLeafsAssets[Random.Range(0, packLeafsAssets.Length)], transform);
                leaf.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                assetsRef.Add(leaf);
                break;
            case PropsType.packLeafs:
                GameObject packLeafs = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                packLeafs.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                packLeafs.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                assetsRef.Add(packLeafs);
                break;
            case PropsType.stone:
                GameObject stone = Instantiate(stoneAssets[Random.Range(0, stoneAssets.Length)], transform);
                stone.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                stone.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                stone.transform.localScale *= Random.Range(.8f, 1.2f);
                assetsRef.Add(stone);
                break;
            case PropsType.none:
                break;
        }
    }

    PropsType GetRandomType()
    {
        int rand = Random.Range(0, 100);
        if (rand < 15)
            return PropsType.grass;
        if (rand >= 15 && rand < 25)
            return PropsType.stone;
        if (rand >= 25 && rand < 35)
            return PropsType.leaf;
        if (rand >= 35 && rand < 50)
            return PropsType.packLeafs;
        return PropsType.none;
    }

     enum PropsType{
        grass, leaf, packLeafs, stone, none
     }

    protected override void GameStart()
    {
        EditorStart();
    }
}