using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody Rigidbody;

    public float Damage;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
