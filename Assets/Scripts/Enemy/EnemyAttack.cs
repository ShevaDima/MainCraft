using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(EnemyAnimator))]
public class EnemyAttack : MonoBehaviour
{
  public EnemyAnimator Animator;

  public EnemyProjectile Projectile;

  public Transform SpawnPosition;

  public float Force;

  public float AttackCooldown = 3f;
  public float Damage = 10f;

  public Transform _heroTransform;
  
  private float _attackCooldown;
  private bool _isAttacking;
  private bool _attackIsActive;

  private void Awake()
  {
    _heroTransform = FindObjectOfType<PlayerController>().transform;
    _attackIsActive = true;
  }

  private void Update()
  {
    UpdateCooldown();

    if (CanAttack())
      StartAttack();
  }


  private Vector3 PickDirection(Transform heroTransform)
  {
    Vector3 direction = heroTransform.transform.position - SpawnPosition.position;
    direction.Normalize();

    return direction;
  }

  private void StartAttack()
  {
    transform.LookAt(_heroTransform);
    Animator.PlayAttack();

    _isAttacking = true;
  }

  //Invoked from Unity Animator
  public void OnAttack()
  {
    EnemyProjectile projectile = Instantiate(Projectile, SpawnPosition.position, Quaternion.identity);
    Vector3 direction = PickDirection(_heroTransform);

    projectile.Damage = Damage;
    projectile.Rigidbody.AddForce(direction * Force);
  }

  //Invoked from Unity Animator
  public void OnAttackEnded()
  {
    _attackCooldown = AttackCooldown;
    _isAttacking = false;
  }

  private bool CooldownIsUp()
  {
    return _attackCooldown <= 0f;
  }

  private void UpdateCooldown()
  {
    if (!CooldownIsUp())
      _attackCooldown -= Time.deltaTime;
  }

  private bool CanAttack()
  {
    return _attackIsActive && CooldownIsUp() && !_isAttacking;
  }

  public void EnableAttack()
  {
    _attackIsActive = true;
  }

  public void DisableAttack()
  {
    _attackIsActive = false;
  }
}