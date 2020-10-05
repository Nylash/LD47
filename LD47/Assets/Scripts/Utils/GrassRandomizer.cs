using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRandomizer : EnhancedMonoBehaviour
{
    [SerializeField] private GameObject[] grassAssets = null;
    [SerializeField] private GameObject[] leafAssets = null;

    protected override void EditorStart()
    {
        if (!GetComponentInParent<VictoryButton>())
        {
            for (float x = -.4f; x < .45f; x += .2f)
            {
                for (float z = -.4f; z < .45f; z += .2f)
                {
                    int i = Random.Range(0, 5);
                    if (i == 0 || i == 1)
                    {
                        GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                        grass.transform.localPosition += new Vector3(x + Random.Range(-.025f, .025f), 0, z + Random.Range(-.025f, .025f));
                    }
                    else
                    {
                        GameObject leaf = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                        leaf.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                        leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                    }

                }
            }
        }
        else
        {
            for (float x = -.4f; x < .45f; x += .2f)
            {
                float z = -.4f;
                int i = Random.Range(0, 5);
                if (i == 0 || i == 1)
                {
                    GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                    grass.transform.localPosition += new Vector3(x + Random.Range(-.025f, .025f), 0, z + Random.Range(-.025f, .025f));
                }
                else
                {
                    GameObject leaf = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                    leaf.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                    leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                }
                float zBis = .4f;
                int iBis = Random.Range(0, 3);
                if (iBis == 0 || iBis == 1)
                {
                    GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                    grass.transform.localPosition += new Vector3(x + Random.Range(-.025f, .025f), 0, zBis + Random.Range(-.025f, .025f));
                }
                else
                {
                    GameObject leaf = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                    leaf.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, zBis + Random.Range(-.05f, .05f));
                    leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                }
            }
            for (float z = -.4f; z < .45f; z += .2f)
            {
                float x = -.4f;
                int i = Random.Range(0, 5);
                if (i == 0 || i == 1)
                {
                    GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                    grass.transform.localPosition += new Vector3(x + Random.Range(-.025f, .025f), 0, z + Random.Range(-.025f, .025f));
                }
                else
                {
                    GameObject leaf = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                    leaf.transform.localPosition += new Vector3(x + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                    leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                }
                float xBis = .4f;
                int iBis = Random.Range(0, 3);
                if (iBis == 0 || iBis == 1)
                {
                    GameObject grass = Instantiate(grassAssets[Random.Range(0, grassAssets.Length)], transform);
                    grass.transform.localPosition += new Vector3(xBis + Random.Range(-.025f, .025f), 0, z + Random.Range(-.025f, .025f));
                }
                else
                {
                    GameObject leaf = Instantiate(leafAssets[Random.Range(0, leafAssets.Length)], transform);
                    leaf.transform.localPosition += new Vector3(xBis + Random.Range(-.05f, .05f), 0, z + Random.Range(-.05f, .05f));
                    leaf.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                }
            }
        }
    }
}