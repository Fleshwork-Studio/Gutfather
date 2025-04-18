using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDeathEvent;
    [SerializeField] private HealthSystem healthSystem;
    void Awake()
    {
        healthSystem.OnDamaged += OnDamaged;
        healthSystem.OnDeath += OnDeath;
    }
    public virtual IEnumerator MakeMove()
    {
        yield return null;
    }
    public virtual IEnumerator InflictDamage(int damageAmount)
    {
        healthSystem.InflictDamage(damageAmount);
        yield return null;
    }
    protected virtual void OnDamaged()
    {
        Debug.Log("Enemy took damage");
    }
    protected virtual void OnDeath()
    {
        Debug.Log("Enemy died");
        OnDeathEvent?.Invoke();

        // TODO: Remove destroy method
        Destroy(gameObject);
    }
    public bool IsAlive() => healthSystem.IsAlive();
}
