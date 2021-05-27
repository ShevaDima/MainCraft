﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private const int visibilityDistance = 30;
    private Transform playerT;
    private bool isVisible;
    private Vector3 chunkPos;

    void Start()
    {
        playerT = GameObject.Find("Player").transform;
        chunkPos = transform.position;
        isVisible = true;
    }

    void Update()
    {
        float distance = Vector3.Distance(chunkPos, new Vector3(playerT.position.x, 0f, playerT.position.z));

        if (distance > visibilityDistance && isVisible)
        {
            SetActivity(false);
        }
        if (distance <= visibilityDistance && !isVisible)
        {
            SetActivity(true);
        }
    }

    private void SetActivity(bool isActive)
    {
        int childrenCount = transform.childCount;

        for (int i = 0; i < childrenCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
        isVisible = isActive;
    }
}
