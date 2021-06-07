using System;
using UnityEngine;

namespace Player
{
  public class PlayerHealth : MonoBehaviour
  {
    public float Current;
    public float Max;

    public event Action Changed;

    private void Awake()
    {
      Current = Max;
    }

    public void TakeDamage(float damage)
    {
      Debug.Log(damage);
      Current -= damage;

      Changed?.Invoke();
    }
  }
}