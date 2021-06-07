using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody Rigidbody;

    public float Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
