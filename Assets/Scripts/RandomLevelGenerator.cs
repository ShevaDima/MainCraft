using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPref, grassPref, chestPref, badrockPref, treePref;
    private int baseHeight = 2,
                maxBlocksCountY = 10,
                chunkSize = 16,
                perlinNoiseSensetivity = 25,
                chunkCount = 4;
    private float seedX, seedY;


    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private List<GameObject> chunksList = new List<GameObject>();

    void Start()
    {
        seedX = Random.Range(0, 10);
        seedY = Random.Range(0, 10);

        for (int x = 0; x <chunkCount; x++)
        {
            for (int z = 0; z <chunkCount; z++)
            {
                CreateChunk(x, z);
            }
        }
    }


    void Update()
    {
        
    }

    private void CreateChunk(int chunkNumX, int chunkNumZ)
    {
        GameObject chunk = new GameObject();
        chunksList.Add(chunk);

        float chunkX = chunkNumX * chunkSize + chunkSize / 2;
        float chunkZ = chunkNumZ * chunkSize + chunkSize / 2;
        chunk.transform.position = new Vector3(chunkX, 0f, chunkZ);
        chunk.name = "chunk: " + chunkX + ", " + chunkZ;
        chunk.AddComponent<MeshFilter>();
        chunk.AddComponent<MeshRenderer>();
        chunk.AddComponent<Chunk>();

        for (int x = chunkNumX * chunkSize; x < chunkNumX * chunkSize + chunkSize; x++)
        {
            for (int z = chunkNumZ * chunkSize; z < chunkNumZ * chunkSize + chunkSize; z++)
            {
                float xSample = seedX + (float)x / perlinNoiseSensetivity;
                float ySample = seedY + (float)z / perlinNoiseSensetivity;
                float sample = Mathf.PerlinNoise(xSample, ySample);
                int height = baseHeight + (int)(sample * maxBlocksCountY);

                for (int y = 0; y < height; y++)
                {
                    GameObject temp;
                    if (y == 0)
                    {
                        temp = Instantiate(badrockPref, new Vector3(x, y, z), Quaternion.identity);
                    }
                    else if (y == height - 1)
                    {
                        temp = Instantiate(grassPref, new Vector3(x, y, z), Quaternion.identity);

                        int createChestChance = Random.Range(0, 100);
                        if (createChestChance > 98)
                        {
                            GameObject temp1 = Instantiate(chestPref, new Vector3(x, height, z), Quaternion.identity);
                            temp1.transform.SetParent(chunk.transform); 
                        }
                        float createTreeChance = Random.Range(0f,100f);
                        if (createTreeChance > 99.5f)
                        {
                            GameObject temp1 = Instantiate(treePref, new Vector3(x, height, z), Quaternion.identity);
                            temp1.transform.SetParent(chunk.transform); 
                        }
                    }
                    else
                    {
                        temp = Instantiate(groundPref, new Vector3(x, y, z), Quaternion.identity);
                    }
                    temp.transform.SetParent(chunk.transform);   
                }
            }
        }
    }

    public void CreateChest(int x, int y, int z)
    {
        int createChestChance = Random.Range(0, 100);
        if (createChestChance > 98)
        {
            Instantiate(chestPref, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}
