using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth=100;
    private int health;
    private bool isInvulnerable = false;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => health == 0;
    
    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealtDamage(int damage)
    {
        if (this.isInvulnerable)
        {
            return;
        }
        if (health <= 0) return;
        if(health>=1)health -= damage;
        
        if(health<=0)OnDie?.Invoke();
        Debug.Log(health);

        OnTakeDamage?.Invoke();
    }

}
