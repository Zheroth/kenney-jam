﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem smoke;
    [SerializeField]
    private float smokeThresshold;

    [SerializeField]
    private Cinemachine.CinemachineImpulseSource onHitImpulse;

    [SerializeField] private int maxHP;
    [SerializeField] private bool invulnerable;

    [SerializeField] private ParticleSystem hitEffect;

    public UnityEvent OnHit;
    public UnityEvent OnHitWhileInvulnerable;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    public delegate void OnHPChanged(int currentHP, int maxHP, float hpPercentage);
    public OnHPChanged onHpChanged;

    private int currentHP;

    public float HealthPercentage
    { get { return (float)currentHP / (float)maxHP; } }

    public bool IsAlive
    {
        get { return currentHP > 0; }

    }

    public void Start()
    {
        ResetHP();
    }

    public void TakeDamage(int damage, Action OnDead = null)
    {
        if (invulnerable)
        {
            OnHitWhileInvulnerable?.Invoke();
        }
        else
        {
            if(currentHP <= 0)
            {
                return;
            }

            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            if(smoke!=null)
            {
                if(currentHP <= smokeThresshold)
                {
                    if(!smoke.isPlaying)
                    {
                        smoke.Play(true);
                    }
                }
                if(currentHP > smokeThresshold)
                {
                    if (smoke.isPlaying)
                    {
                        smoke.Stop(true);
                    }
                }
            }

            onHitImpulse.GenerateImpulse();
            OnHit.Invoke();
            onHpChanged?.Invoke(currentHP, maxHP, HealthPercentage);

            if (currentHP == 0)
            {
                OnDead?.Invoke();
                OnDeath.Invoke();
            }

            hitEffect.Play();
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
        if (onHpChanged != null)
        {
            onHpChanged(currentHP, maxHP, HealthPercentage);
        }
    }

    public void ResetHP()
    {
        currentHP = maxHP;
        if (onHpChanged != null)
        {
            onHpChanged(currentHP, maxHP, HealthPercentage);
        }
    }
}
