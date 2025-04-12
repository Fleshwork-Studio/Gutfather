using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    void Awake()
    {
        healthSystem.OnDamaged += OnDamaged;
        healthSystem.OnDeath += OnDeath;
    }
    public virtual IEnumerator MakeMove()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Enemy made a move");
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
    }
    public bool IsAlive() => healthSystem.IsAlive();
}
