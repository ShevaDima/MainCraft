using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
  public class EnemyDeath : MonoBehaviour
  {
    public EnemyHealth Health;
    public EnemyAnimator Animator;

    public event Action Happened;

    private void Start()
    {
      Health.Changed += HealthChanged;
    }

    private void OnDestroy()
    {
      Health.Changed -= HealthChanged;
    }

    private void HealthChanged()
    {
      if (Health.Current <= 0)
        Die();
    }

    private void Die()
    {
      Health.Changed -= HealthChanged;
      Animator.PlayDeath();
      
      StartCoroutine(DestroyTimer());
      Happened?.Invoke();
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(3f);
      Destroy(gameObject);
    }
  }
}