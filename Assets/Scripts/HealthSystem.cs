using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDeath;
    [SerializeField] private int maxHP;

    private int currentHP;
    private bool isDead = false;
    void Awake()
    {
        currentHP = maxHP;
    }
    public void InflictDamage(int damageAmount)
    {
        if (isDead) return;

        currentHP -= damageAmount;

        OnDamaged?.Invoke();

        if (currentHP <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }
    public bool IsAlive() => !isDead;
    public void SetMaxHP(int maxHP, bool restoreAllHealth)
    {
        this.maxHP = maxHP;

        if (restoreAllHealth)
            currentHP = this.maxHP;
    }
}
