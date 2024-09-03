using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealtDamage(int damage)
    {
        if (health >= 1) health -= damage;
        if (health <= 0) Destroy(gameObject);
        Debug.Log(health);
    }
}
