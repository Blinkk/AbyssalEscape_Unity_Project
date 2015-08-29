using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        if (transform.parent)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
