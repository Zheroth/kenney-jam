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

    private int currentHP;

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
    }

    public void ResetHP()
    {
        currentHP = maxHP;
    }
}
