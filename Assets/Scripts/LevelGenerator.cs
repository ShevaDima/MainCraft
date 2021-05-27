using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject cube;

    private const int pyramidCount = 5;
    private const int pyramidHeight = 6;
    private const int pyramidBase = pyramidHeight * 2 - 1;

    void Start()
    {
        for (int x = 0; x < pyramidCount; x++)
        {
            for (int z = 0; z < pyramidCount; z++)
            {
                CreatePyramid(new Vector3(x * pyramidBase, 0, z * pyramidBase)); 
            }
        }
    }


    void Update()
    {
        
    }

    void CreatePyramid(Vector3 pos)
    {
        int offsetX = 0, offsetZ = 0;

        for (int y = 0; y < pyramidHeight; y++)
        {
            for (int x = (int)pos.x + offsetX; x < pos.x + pyramidBase - offsetX; x++)
            {
                for (int z = (int)pos.z + offsetZ; z < pos.z + pyramidBase - offsetZ; z++)
                {
                    Instantiate(cube, new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), Quaternion.identity);
                }
            }
            offsetX++;
            offsetZ++;
        }
    }
}
