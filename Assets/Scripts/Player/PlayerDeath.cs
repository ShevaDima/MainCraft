using System;
using System.Collections;
using UnityEngine;

namespace Player
{
  public class PlayerDeath : MonoBehaviour
  {
    public PlayerHealth Health;
    public PlayerController PlayerController;

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

      //StartCoroutine(DestroyTimer());

      PlayerController.enabled = false;
      Happened?.Invoke();
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(3f);
      //Destroy(gameObject);
    }
  }
}