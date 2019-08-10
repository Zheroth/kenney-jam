using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private bool invulnerable;

    public UnityEvent OnHit;
    public UnityEvent OnHitWhileInvulnerable;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    public delegate void OnHPChanged(int currentHP, int maxHP, float hpPercentage);
    public OnHPChanged onHpChanged;

    private int currentHP;

    public float HealthPercentage
    { get { return (float)currentHP / (float)maxHP; } }

    public void Start()
    {
        ResetHP();
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable)
        {
            OnHitWhileInvulnerable?.Invoke();
        }
        else
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            OnHit.Invoke();
            onHpChanged(currentHP, maxHP, HealthPercentage);

            if (currentHP == 0)
            {
                OnDeath.Invoke();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
            OnHeal?.Invoke();
        }
        onHpChanged(currentHP, maxHP, HealthPercentage);
    }

    public void ResetHP()
    {
        currentHP = maxHP;
        onHpChanged(currentHP, maxHP, HealthPercentage);
    }
}
