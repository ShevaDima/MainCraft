using System;
using Enemy;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
  private static readonly int Attack = Animator.StringToHash("Attack_1");

  private readonly int _idleStateHash = Animator.StringToHash("idle");
  private readonly int _attackStateHash = Animator.StringToHash("attack01");
  private readonly int _walkingStateHash = Animator.StringToHash("Move");
  private readonly int _deathStateHash = Animator.StringToHash("die");

  private Animator _animator;

  private void Awake() 
  {
    _animator = GetComponent<Animator>();
  }
   
  
  public void PlayAttack()
  {
     _animator.SetTrigger(Attack);
  }
}