using System;
using UnityEngine;

namespace Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class EnemyHealth : MonoBehaviour
  {
    public EnemyAnimator Animator;

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
      Animator.PlayHit();
      
      Changed?.Invoke();
    }
  }
}